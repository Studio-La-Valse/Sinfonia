using Sinfonia.EntityFramework;
using Sinfonia.EntityFramework.Entities;
using System;

namespace Sinfonia.Implementations
{
    internal class ScoreDocumentRepository : IScoreDocumentRepository
    {
        private readonly ScoreDocumentContext scoreDocumentContext;

        public ScoreDocumentRepository(ScoreDocumentContext scoreDocumentContext)
        {
            this.scoreDocumentContext = scoreDocumentContext;
        }

        public IEnumerable<Guid> AvailableScores()
        {
            return scoreDocumentContext.ScoreDocuments.Select(e => e.Id);
        }

        public ScoreDocumentMemento Get(Guid guid)
        {
            var document = scoreDocumentContext.ScoreDocuments.First(d => d.Id == guid);

            InstrumentMeasureMemento instrumentMeasureFromSource(InstrumentMeasureModel instrumentMeasureModel)
            {
                var voices = new List<RibbonMeasureVoiceMemento>();
                foreach(var voiceGroup in instrumentMeasureModel.MeasureBlocks.GroupBy(b => b.Voice))
                {
                    var voice = voiceGroup.Key;

                    var voiceMemento = new RibbonMeasureVoiceMemento()
                    {
                        Voice = voice,
                        MeasureBlocks = voiceGroup.Select(measureBlockFromSource).ToList()
                    };

                    voices.Add(voiceMemento);
                }

                return new InstrumentMeasureMemento()
                {
                    Guid = instrumentMeasureModel.Id,
                    MeasureIndex = instrumentMeasureModel.ScoreMeasure.IndexInScore,
                    RibbonIndex = instrumentMeasureModel.InstrumentRibbon.IndexInScore,
                    VoiceGroups = voices
                };
            }

            MeasureBlockMemento measureBlockFromSource(MeasureBlockModel measureBlockModel)
            {
                var memento = new MeasureBlockMemento()
                {
                    Guid = measureBlockModel.Id,
                    Chords = measureBlockModel.Chords.Select(chordFromSource).ToList(),
                    Duration = new RythmicDuration(measureBlockModel.Duration, measureBlockModel.Dots),
                    Grace = false
                };

                return memento;
            }

            ChordMemento chordFromSource(ChordModel chordModel)
            {
                var chord = new ChordMemento()
                {
                    Guid = chordModel.Id,
                    Notes = chordModel.Notes.Select(noteFromSource).ToList(),
                    RythmicDuration = new RythmicDuration(chordModel.Duration, chordModel.Dots)
                };

                return chord;
            }

            NoteMemento noteFromSource(NoteModel noteModel)
            {
                var note = new NoteMemento()
                {
                    Guid = noteModel.Id,
                    Pitch = new Pitch(new Step(noteModel.Step, noteModel.Shifts), noteModel.Octave)
                };

                return note;
            }

            var memento = new ScoreDocumentMemento()
            {
                Guid = Guid.NewGuid(),
                InstrumentRibbons = document.InstrumentRibbons.Select(r =>
                {
                    if(!Instrument.TryGetFromName(r.Instrument, out var instrument))
                    {
                        throw new Exception();
                    }

                    return new InstrumentRibbonMemento()
                    {
                        Guid = r.Id,
                        Instrument = instrument,
                        IndexInScore = r.IndexInScore,
                        InstrumentMeasures = r.InstrumentMeasures.Select(instrumentMeasureFromSource).ToList()
                    };
                }).ToList(),
                ScoreMeasures = document.ScoreMeasures.Select(r =>
                {
                    return new ScoreMeasureMemento()
                    {
                        Guid = r.Id,
                        Measures = r.InstrumentMeasures.Select(instrumentMeasureFromSource).ToList(),
                        TimeSignature = new TimeSignature(r.TimeSignatureNumerator, r.TimeSignatureDenominator),
                        IndexInScore = r.IndexInScore
                    };
                }).ToList(),
                Layout = null
            };

            return memento;
        }

        public ScoreDocumentMemento RestoreLayout(ScoreDocumentMemento scoreDocumentMemento)
        {
            var layout = scoreDocumentContext.ScoreDocumentLayouts.First(d => d.ScoreDocument.Id == scoreDocumentMemento.Guid);

            var copy = new ScoreDocumentMemento()
            {
                InstrumentRibbons = scoreDocumentMemento.InstrumentRibbons,
                Guid = scoreDocumentMemento.Guid,
                ScoreMeasures = scoreDocumentMemento.ScoreMeasures,
                Layout = new ScoreDocumentLayoutMemento()
                {
                    Scale = layout.Scale
                }
            };

            return scoreDocumentMemento;
        }

        public void Upload(IScoreDocumentReader documentReader)
        {
            var instrumentMeasureModels = new List<InstrumentMeasureModel>();   

            foreach(var instrumentMeasure in documentReader.ReadScoreMeasures().SelectMany(e => e.ReadMeasures()))
            {
                var measureBlocks = fromInstrumentMeasure(instrumentMeasure).ToList();

                var model = new InstrumentMeasureModel()
                {
                    Id = instrumentMeasure.Guid,
                    MeasureBlocks = measureBlocks,
                    ScoreMeasureIndex = instrumentMeasure.MeasureIndex,
                    InstrumentRibbonIndex = instrumentMeasure.RibbonIndex,
                };

                instrumentMeasureModels.Add(model);
            }

            IEnumerable<MeasureBlockModel> fromInstrumentMeasure(IInstrumentMeasureReader instrumentMeasure)
            {
                var measureBlocks = new List<MeasureBlockModel>();

                foreach (var voice in instrumentMeasure.ReadVoices())
                {
                    foreach (var block in instrumentMeasure.ReadBlockChainAt(voice).ReadBlocks())
                    {
                        var measureBlockModel = new MeasureBlockModel()
                        {
                            Voice = voice,
                            Dots = block.RythmicDuration.Dots,
                            Duration = block.RythmicDuration.PowerOfTwo,
                            Id = block.Guid,
                            Chords = block.ReadChords().Select(fromChord).ToList()
                        };

                        measureBlocks.Add(measureBlockModel);
                    }
                }

                return measureBlocks;
            }

            ChordModel fromChord(IChordReader chord)
            {
                var chordModel = new ChordModel()
                {
                    Dots = chord.RythmicDuration.Dots,
                    Duration = chord.RythmicDuration.PowerOfTwo,
                    Id = chord.Guid,
                    Notes = chord.ReadNotes().Select(n =>
                    {
                        return new NoteModel()
                        {
                            Id = n.Guid,
                            Octave = n.Pitch.Octave,
                            Shifts = n.Pitch.Shift,
                            Step = n.Pitch.Step.StepsFromC
                        };
                    }).ToArray()
                };

                return chordModel;
            }

            var document = new ScoreDocumentModel()
            {
                Id = documentReader.Guid,
                ScoreMeasures = documentReader.ReadScoreMeasures().Select(m =>
                {
                    return new ScoreMeasureModel()
                    {
                        Id = m.Guid,
                        InstrumentMeasures = instrumentMeasureModels.Where(_m => _m.ScoreMeasureIndex == m.IndexInScore).ToArray(),
                        TimeSignatureNumerator = m.TimeSignature.Numerator,
                        TimeSignatureDenominator = m.TimeSignature.Denominator
                    };
                }).ToList(),
                InstrumentRibbons = documentReader.ReadInstrumentRibbons().Select(r =>
                {
                    return new InstrumentRibbonModel()
                    {
                        Id = r.Guid,
                        Instrument = r.Instrument.Name,
                        InstrumentMeasures = instrumentMeasureModels.Where(_r => _r.InstrumentRibbonIndex == r.IndexInScore).ToArray()
                    };
                }).ToList()
            };

            scoreDocumentContext.ScoreDocuments.Add(document);

            var layout = new ScoreDocumentLayoutModel()
            {
                Id = Guid.NewGuid(),
                ScoreDocumentId = document.Id,
                ScoreMeasureLayouts = document.ScoreMeasures.Select(m =>
                {
                    return new ScoreMeasureLayoutModel()
                    {
                        Id = Guid.NewGuid(),
                        ScoreMeasureId = m.Id,
                        Width = 150
                    };
                }).ToArray()
            };

            scoreDocumentContext.ScoreDocumentLayouts.Add(layout);  

            scoreDocumentContext.SaveChanges();
        }

        public void Upload(IScoreDocumentLayout scoreDocumentLayout)
        {

        }

        public void Dispose()
        {
            scoreDocumentContext.Dispose();
        }
    }
}
