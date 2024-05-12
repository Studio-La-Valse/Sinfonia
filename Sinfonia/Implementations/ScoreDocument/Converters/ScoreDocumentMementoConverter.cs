using StudioLaValse.ScoreDocument.Models.Entities;

namespace Sinfonia.Implementations.ScoreDocument.Converters
{
    public class ScoreDocumentMementoConverter
    {
        private readonly ScoreDocumentLayoutMementoConverter layoutConverter;
        private readonly ScoreElementMementoConverter elementConverter;

        public ScoreDocumentMementoConverter(ScoreDocumentLayoutMementoConverter layoutConverter, ScoreElementMementoConverter elementConverter)
        {
            this.layoutConverter = layoutConverter;
            this.elementConverter = elementConverter;
        }

        public ScoreDocumentMemento Convert(ScoreDocumentModel scoreDocument)
        {
            var instrumentRibbons = scoreDocument.InstrumentRibbons.Select(Convert).ToList();
            var scoreMeasures = scoreDocument.ScoreMeasures.Select(Convert).ToList();
            var layout = scoreDocument.Layout;
            var _layout = layoutConverter.Convert(layout);
            var memento = new ScoreDocumentMemento()
            {
                Id = scoreDocument.Id,
                InstrumentRibbons = instrumentRibbons,
                ScoreMeasures = scoreMeasures,
                Layout = _layout
            };
            return memento;
        }
        public ScoreDocumentModel ConvertBack(ScoreDocumentMemento scoreDocument)
        {
            var instrumentRibbons = scoreDocument.InstrumentRibbons.Select(ConvertBack).ToList();
            var scoreMeasures = scoreDocument.ScoreMeasures.Select(ConvertBack).ToList();
            var layout = layoutConverter.ConvertBack(scoreDocument.Layout);
            var memento = new ScoreDocumentModel()
            {
                Id = scoreDocument.Id,
                InstrumentRibbons = instrumentRibbons,
                ScoreMeasures = scoreMeasures,
                Layout = layout
            };
            return memento;
        }


        public InstrumentRibbonMemento Convert(InstrumentRibbonModel instrumentRibbon)
        {
            var instrument = elementConverter.Convert(instrumentRibbon.Instrument);
            var instrumentMeasures = instrumentRibbon.InstrumentMeasures.Select(Convert).ToList();
            var layout = layoutConverter.Convert(instrumentRibbon.Layout);
            var _instrumentRibbon = new InstrumentRibbonMemento()
            {
                Id = instrumentRibbon.Id,
                Instrument = instrument,
                IndexInScore = instrumentRibbon.IndexInScore,
                InstrumentMeasures = instrumentMeasures,
                Layout = layout
            };
            return _instrumentRibbon;
        }
        public InstrumentRibbonModel ConvertBack(InstrumentRibbonMemento instrumentRibbon)
        {
            var instrument = elementConverter.ConvertBack(instrumentRibbon.Instrument);
            var scoreMeasures = instrumentRibbon.InstrumentMeasures.Select(ConvertBack).ToList();
            var layout = layoutConverter.ConvertBack(instrumentRibbon.Layout);
            var _instrumentRibbon = new InstrumentRibbonModel()
            {
                Id = instrumentRibbon.Id,
                Instrument = instrument,
                IndexInScore = instrumentRibbon.IndexInScore,
                InstrumentMeasures = scoreMeasures,
                Layout = layout
            };
            return _instrumentRibbon;
        }


        public ScoreMeasureMemento Convert(ScoreMeasureModel scoreMeasure)
        {
            var measures = scoreMeasure.InstrumentMeasures.Select(Convert).ToList();
            var timeSignature = elementConverter.Convert(scoreMeasure.TimeSignature);
            var layout = scoreMeasure.Layout;
            var _layout = layoutConverter.Convert(layout);
            var _scoreMeasure = new ScoreMeasureMemento()
            {
                Id = scoreMeasure.Id,
                InstrumentMeasures = measures,
                TimeSignature = timeSignature,
                IndexInScore = scoreMeasure.IndexInScore,
                Layout = _layout
            };
            return _scoreMeasure;
        }
        public ScoreMeasureModel ConvertBack(ScoreMeasureMemento scoreMeasure)
        {
            var measures = scoreMeasure.InstrumentMeasures.Select(ConvertBack).ToList();
            var timeSignature = elementConverter.ConvertBack(scoreMeasure.TimeSignature);
            var layout = layoutConverter.ConvertBack(scoreMeasure.Layout);
            var _scoreMeasure = new ScoreMeasureModel()
            {
                Id = scoreMeasure.Id,
                InstrumentMeasures = measures,
                TimeSignature = timeSignature,
                IndexInScore = scoreMeasure.IndexInScore,
                Layout = layout
            };
            return _scoreMeasure;
        }


        public InstrumentMeasureMemento Convert(InstrumentMeasureModel instrumentMeasureModel)
        {
            var voices = new List<RibbonMeasureVoiceMemento>();
            foreach (var voiceGroup in instrumentMeasureModel.MeasureBlocks.GroupBy(b => b.Voice))
            {
                var voice = voiceGroup.Key;
                var measureBlocks = voiceGroup.Select(Convert).ToList();
                var voiceMemento = new RibbonMeasureVoiceMemento()
                {
                    Voice = voice,
                    MeasureBlocks = measureBlocks
                };

                voices.Add(voiceMemento);
            }

            var _layout = layoutConverter.Convert(instrumentMeasureModel.Layout);
            return new InstrumentMeasureMemento()
            {
                Id = instrumentMeasureModel.Id,
                MeasureIndex = instrumentMeasureModel.ScoreMeasureIndex,
                RibbonIndex = instrumentMeasureModel.InstrumentRibbonIndex,
                VoiceGroups = voices,
                Layout = _layout
            };
        }
        public InstrumentMeasureModel ConvertBack(InstrumentMeasureMemento instrumentMeasureModel)
        {
            var measureBlocks = new List<MeasureBlockModel>();
            foreach (var voice in instrumentMeasureModel.VoiceGroups)
            {
                foreach (var block in voice.MeasureBlocks)
                {
                    var blockModel = ConvertBack(block);
                    measureBlocks.Add(blockModel);
                }
            }

            var layout = layoutConverter.ConvertBack(instrumentMeasureModel.Layout);
            return new InstrumentMeasureModel()
            {
                Id = instrumentMeasureModel.Id,
                ScoreMeasureIndex = instrumentMeasureModel.MeasureIndex,
                InstrumentRibbonIndex = instrumentMeasureModel.RibbonIndex,
                MeasureBlocks = measureBlocks,
                Layout = layout
            };
        }


        public MeasureBlockMemento Convert(MeasureBlockModel measureBlockModel)
        {
            var chords = measureBlockModel.Chords.Select(Convert).ToList();
            var duration = elementConverter.Convert(measureBlockModel.Duration);
            var layout = layoutConverter.Convert(measureBlockModel.Layout);
            var memento = new MeasureBlockMemento()
            {
                Id = measureBlockModel.Id,
                Chords = chords,
                Duration = duration,
                Layout = layout,
                Voice = measureBlockModel.Voice
            };

            return memento;
        }
        public MeasureBlockModel ConvertBack(MeasureBlockMemento measureBlockModel)
        {
            var chords = measureBlockModel.Chords.Select(ConvertBack).ToList();
            var duration = elementConverter.ConvertBack(measureBlockModel.Duration);
            var layout = layoutConverter.ConvertBack(measureBlockModel.Layout);
            var memento = new MeasureBlockModel()
            {
                Id = measureBlockModel.Id,
                Chords = chords,
                Duration = duration,
                Layout = layout,
                Voice = measureBlockModel.Voice
            };

            return memento;
        }


        public ChordMemento Convert(ChordModel chordModel)
        {
            var notes = chordModel.Notes.Select(Convert).ToList();
            var duration = elementConverter.Convert(chordModel.RythmicDuration);
            var layout = layoutConverter.Convert(chordModel.Layout);
            var chord = new ChordMemento()
            {
                Id = chordModel.Id,
                Notes = notes,
                RythmicDuration = duration,
                Layout = layout
            };

            return chord;
        }
        public ChordModel ConvertBack(ChordMemento chordModel)
        {
            var notes = chordModel.Notes.Select(ConvertBack).ToList();
            var duration = elementConverter.ConvertBack(chordModel.RythmicDuration);
            var layout = layoutConverter.ConvertBack(chordModel.Layout);
            var chord = new ChordModel()
            {
                Id = chordModel.Id,
                Notes = notes,
                RythmicDuration = duration,
                Layout = layout
            };

            return chord;
        }


        public NoteMemento Convert(NoteModel noteModel)
        {
            var pitch = elementConverter.Convert(noteModel.Pitch);
            var layout = layoutConverter.Convert(noteModel.Layout);
            var note = new NoteMemento()
            {
                Id = noteModel.Id,
                Pitch = pitch,
                Layout = layout
            };

            return note;
        }
        public NoteModel ConvertBack(NoteMemento noteModel)
        {
            var pitch = elementConverter.ConvertBack(noteModel.Pitch);
            var layout = layoutConverter.ConvertBack(noteModel.Layout);
            var note = new NoteModel()
            {
                Id = noteModel.Id,
                Pitch = pitch,
                Layout = layout
            };

            return note;
        }
    }
}
