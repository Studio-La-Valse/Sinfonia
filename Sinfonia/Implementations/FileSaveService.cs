using Avalonia.Platform.Storage;
using Sinfonia.EntityFramework;
using Sinfonia.EntityFramework.Entities;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Reader;
using System.Text.Json;
using System.Xml.Serialization;
using ColorARGB = Sinfonia.EntityFramework.Classes.ColorARGB;

namespace Sinfonia.Implementations
{
#nullable disable
    public class DocumentModel
    {
        public ScoreDocumentModel ScoreDocument { get; set; }
        public ScoreDocumentStyleTemplate StyleTemplate { get; set; }
    }
#nullable enable

    public class ScoreElementMementoConverter
    {
        public ScoreElementMementoConverter()
        {
            
        }

        public Instrument Convert(EntityFramework.Classes.Instrument instrument)
        {
            var _instrument = Instrument.CreateCustom(instrument.Name, instrument.Clefs.Select(Convert).ToArray());
            return _instrument;
        }
        public EntityFramework.Classes.Instrument ConvertBack(Instrument instrument)
        {
            var _instrument = new EntityFramework.Classes.Instrument()
            {
                Clefs = instrument.DefaultClefs.Select(ConvertBack).ToList(),
                Name = instrument.Name,
            };
            return _instrument;
        }

        public RythmicDuration Convert(EntityFramework.Classes.RythmicDuration rythmicDuration)
        {
            var _rythmicDuration = new RythmicDuration(rythmicDuration.PowerOfTwo, rythmicDuration.Dots);
            return _rythmicDuration;
        }
        public EntityFramework.Classes.RythmicDuration ConvertBack(RythmicDuration rythmicDuration)
        {
            var _rythmicDuration = new EntityFramework.Classes.RythmicDuration()
            {
                PowerOfTwo = rythmicDuration.PowerOfTwo,
                Dots = rythmicDuration.Dots
            };
            return _rythmicDuration;
        }

        public TimeSignature Convert(EntityFramework.Classes.TimeSignature timeSignature)
        {
            var _timeSignature = new TimeSignature(timeSignature.Numerator, timeSignature.Denominator);
            return _timeSignature;
        }
        public EntityFramework.Classes.TimeSignature ConvertBack(TimeSignature timeSignature)
        {
            var _timeSignature = new EntityFramework.Classes.TimeSignature()
            {
                Denominator = timeSignature.Denominator,
                Numerator = timeSignature.Numerator,
            };
            return _timeSignature;
        }

        public KeySignature Convert(EntityFramework.Classes.KeySignature keySignature)
        {
            var step = Convert(keySignature.Step);
            var _keySignature = new KeySignature(step, keySignature.Major ? MajorOrMinor.Major : MajorOrMinor.Minor);
            return _keySignature;
        }
        public EntityFramework.Classes.KeySignature ConvertBack(KeySignature keySignature)
        {
            var step = ConvertBack(keySignature.Origin);
            var major = keySignature.MajorOrMinor == MajorOrMinor.Major ? true : false;
            var _keySignature = new EntityFramework.Classes.KeySignature()
            {
                Major = major,
                Step = step,
            };
            return _keySignature;
        }

        public Clef Convert(string clef)
        {
            var _clef = clef.ToLower() switch
            {
                "treble" => Clef.Treble,
                "bass" => Clef.Bass,
                _ => throw new NotImplementedException()
            };
            return _clef;
        }
        public string ConvertBack(Clef clef)
        {
            return clef.Name.ToString();
        }

        public Step Convert(EntityFramework.Classes.Step step)
        {
            var _step = new Step(step.StepsFromC, step.Shifts);
            return _step;
        }
        public EntityFramework.Classes.Step ConvertBack(Step step)
        {
            var _step = new EntityFramework.Classes.Step()
            {
                StepsFromC = step.StepsFromC,
                Shifts = step.Shifts
            };
            return _step;
        }

        public Pitch Convert(EntityFramework.Classes.Pitch pitch)
        {
            var step = Convert(pitch.Step);
            var _pitch = new Pitch(step, pitch.Octave);
            return _pitch;
        }
        public EntityFramework.Classes.Pitch ConvertBack(Pitch pitch)
        {
            var _pitch = new EntityFramework.Classes.Pitch()
            {
                Octave = pitch.Octave,
                Step = ConvertBack(pitch.Step)
            };
            return _pitch;
        }

        public Position Convert(EntityFramework.Classes.Position position)
        {
            var _position = new Position(position.Numerator, position.Denominator);
            return _position;
        }
        public EntityFramework.Classes.Position ConvertBack(Position position)
        {
            var _position = new EntityFramework.Classes.Position()
            {
                Denominator = position.Denominator,
                Numerator = position.Numerator,
            };
            return _position;
        }
    }



    public class ScoreDocumentLayoutMementoConverter
    {
        private readonly ScoreElementMementoConverter elementConverter;

        public ScoreDocumentLayoutMementoConverter(ScoreElementMementoConverter elementConverter)
        {
            this.elementConverter = elementConverter;
        }

        public ScoreDocumentLayoutMemento Convert(ScoreDocumentLayoutModel layout)
        {
            var foreGround = layout.ForegroundColor is null ? null : Convert(layout.ForegroundColor);
            var pageColor = layout.PageColor is null ? null : Convert(layout.PageColor);
            var _layout = new ScoreDocumentLayoutMemento()
            {
                Id = layout.Id,
                FirstSystemIndent = layout.FirstSystemIndent,
                ForegroundColor = foreGround,
                HorizontalStaffLineThickness = layout.HorizontalStaffLineThickness,
                InstrumentScales = layout.InstrumentScales.ToDictionary(v => v.Key, v => v.Value),
                PageColor = pageColor,
                Scale = layout.Scale,
                StemLineThickness = layout.StemLineThickness,
                VerticalStaffLineThickness = layout.VerticalStaffLineThickness
            };
            return _layout;
        }
        public ScoreDocumentLayoutModel ConvertBack(ScoreDocumentLayoutMemento layout)
        {
            var foreGround = layout.ForegroundColor is null ? null : ConvertBack(layout.ForegroundColor);
            var _layout = new ScoreDocumentLayoutModel()
            {
                Id = layout.Id,
                FirstSystemIndent = layout.FirstSystemIndent,
                ForegroundColor = foreGround,
                HorizontalStaffLineThickness = layout.HorizontalStaffLineThickness,
                InstrumentScales = layout.InstrumentScales.ToDictionary(v => v.Key, v => v.Value),
                PageColor = layout.PageColor is null ? null : ConvertBack(layout.PageColor),
                Scale = layout.Scale,
                StemLineThickness = layout.StemLineThickness,
                VerticalStaffLineThickness = layout.VerticalStaffLineThickness
            };
            return _layout;
        }


        public ScoreMeasureLayoutMemento Convert(ScoreMeasureLayoutModel layout)
        {
            var _layout = new ScoreMeasureLayoutMemento()
            {
                Id = layout.Id,
                KeySignature = layout.KeySignature is null ? null : elementConverter.Convert(layout.KeySignature),
                PaddingLeft = layout.PaddingLeft,
                PaddingBottom = layout.PaddingBottom,
                PaddingRight = layout.PaddingRight,
                Width = layout.Width
            };
            return _layout;
        }
        public ScoreMeasureLayoutModel ConvertBack(ScoreMeasureLayoutMemento layout)
        {
            var _layout = new ScoreMeasureLayoutModel()
            {
                Id = layout.Id,
                KeySignature = layout.KeySignature.HasValue ? elementConverter.ConvertBack(layout.KeySignature.Value) : null,
                PaddingLeft = layout.PaddingLeft,
                PaddingBottom = layout.PaddingBottom,
                PaddingRight = layout.PaddingRight,
                Width = layout.Width
            };
            return _layout;
        }


        public InstrumentRibbonLayoutMemento Convert(InstrumentRibbonLayoutModel layoutModel)
        {
            var layout = new InstrumentRibbonLayoutMemento()
            {
                Id = layoutModel.Id,
                AbbreviatedName = layoutModel.AbbreviatedName,
                Collapsed = layoutModel.Collapsed,
                DisplayName = layoutModel.DisplayName,
                NumberOfStaves = layoutModel.NumberOfStaves,
            };
            return layout;
        }
        public InstrumentRibbonLayoutModel ConvertBack(InstrumentRibbonLayoutMemento layoutModel)
        {
            var layout = new InstrumentRibbonLayoutModel()
            {
                Id = layoutModel.Id,
                AbbreviatedName = layoutModel.AbbreviatedName,
                Collapsed = layoutModel.Collapsed,
                DisplayName = layoutModel.DisplayName,
                NumberOfStaves = layoutModel.NumberOfStaves,
            };
            return layout;
        }


        public InstrumentMeasureLayoutMemento Convert(InstrumentMeasureLayoutModel layoutModel)
        {
            var layout = new InstrumentMeasureLayoutMemento()
            {
                Id = layoutModel.Id,
                ClefChanges = layoutModel.ClefChanges.Select(Convert).ToList(),
                Collapsed = layoutModel.Collapsed,
                NumberOfStaves = layoutModel.NumberOfStaves,
                PaddingBottom = layoutModel.PaddingBottom,
                StaffPaddingBottom = layoutModel.StaffPaddingBottom.ToDictionary(v => v.Key, v => v.Value)
            };
            return layout;
        }
        public InstrumentMeasureLayoutModel ConvertBack(InstrumentMeasureLayoutMemento layoutModel)
        {
            var layout = new InstrumentMeasureLayoutModel()
            {
                Id = layoutModel.Id,
                ClefChanges = layoutModel.ClefChanges.Select(ConvertBack).ToList(),
                Collapsed = layoutModel.Collapsed,
                NumberOfStaves = layoutModel.NumberOfStaves,
                PaddingBottom = layoutModel.PaddingBottom,
                StaffPaddingBottom = layoutModel.StaffPaddingBottom.ToDictionary(v => v.Key, v => v.Value)
            };
            return layout;
        }


        public MeasureBlockLayoutMemento Convert(MeasureBlockLayoutModel measureBlockLayout)
        {
            var _measureBlockLayout = new MeasureBlockLayoutMemento()
            {
                Id = measureBlockLayout.Id,
                BeamAngle = measureBlockLayout.BeamAngle,
                StemLength = measureBlockLayout.StemLength,
            };
            return _measureBlockLayout;
        }
        public MeasureBlockLayoutModel ConvertBack(MeasureBlockLayoutMemento measureBlockLayout)
        {
            var layout = new MeasureBlockLayoutModel()
            {
                Id = measureBlockLayout.Id,
                BeamAngle = measureBlockLayout.BeamAngle,
                StemLength = measureBlockLayout.StemLength
            };
            return layout;
        }


        public ChordLayoutMemento Convert(ChordLayoutModel noteLayout)
        {
            var _noteLayout = new ChordLayoutMemento()
            {
                Id = noteLayout.Id,
                XOffset = noteLayout.XOffset
            };
            return _noteLayout;
        }
        public ChordLayoutModel ConvertBack(ChordLayoutMemento noteLayout)
        {
            var layout = new ChordLayoutModel()
            {
                Id = noteLayout.Id,
                XOffset = noteLayout.XOffset,
            };
            return layout;
        }


        public NoteLayoutMemento Convert(NoteLayoutModel noteLayout)
        {
            var _noteLayout = new NoteLayoutMemento()
            {
                Id = noteLayout.Id,
                ForceAccidental = noteLayout.ForceAccidental.HasValue ? (AccidentalDisplay)noteLayout.ForceAccidental : null,
                Scale = noteLayout.Scale,
                StaffIndex = noteLayout.StaffIndex,
                XOffset = noteLayout.XOffset
            };
            return _noteLayout;
        }
        public NoteLayoutModel ConvertBack(NoteLayoutMemento noteLayout)
        {
            var layout = new NoteLayoutModel()
            {
                Id = noteLayout.Id,
                ForceAccidental = noteLayout.ForceAccidental.HasValue ? (int)noteLayout.ForceAccidental : null,
                Scale = noteLayout.Scale,
                StaffIndex = noteLayout.StaffIndex,
                XOffset = noteLayout.XOffset,
            };
            return layout;
        }


        public ClefChange Convert(EntityFramework.Classes.ClefChange clefChange)
        {
            var _clefChange = new ClefChange(elementConverter.Convert(clefChange.Clef), clefChange.StaffIndex, elementConverter.Convert(clefChange.Position));
            return _clefChange;
        }
        public EntityFramework.Classes.ClefChange ConvertBack(ClefChange clefChange)
        {
            var _clefChange = new EntityFramework.Classes.ClefChange()
            {
                Clef = elementConverter.ConvertBack(clefChange.Clef),
                Position = elementConverter.ConvertBack(clefChange.Position),
                StaffIndex = clefChange.StaffIndex
            };
            return _clefChange;
        }


        public StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB Convert(ColorARGB color)
        {
            var _color = new StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B,
            };
            return _color;
        }
        public ColorARGB ConvertBack(StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB color)
        {
            var _color = new ColorARGB()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B,
            };
            return _color;
        }
    }

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
                foreach(var block in voice.MeasureBlocks)
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

    public class FileSaveService : IFileSaveService
    {
        private readonly ScoreDocumentMementoConverter documentMementoConverter;
        private readonly MainWindow mainWindow;

        public FileSaveService(ScoreDocumentMementoConverter documentMementoConverter, MainWindow mainWindow)
        {
            this.documentMementoConverter = documentMementoConverter;
            this.mainWindow = mainWindow;
        }



        public (ScoreDocumentMemento, ScoreDocumentStyleTemplate) Get()
        {
            var task = mainWindow.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions()
            {
                Title = "Open",
                AllowMultiple = false,
                FileTypeFilter = [new FilePickerFileType("Sinfonia files") { Patterns = ["*.sin"] }]
            });
            var result = AsyncHelper.RunSync(() => task);
            if(result.Count == 0)
            {
                throw new Exception();
            }

            var file = result[0];
            using var stream = AsyncHelper.RunSync(file.OpenReadAsync);
            var documentModel = JsonSerializer.Deserialize<DocumentModel>(stream) ?? throw new Exception();
            var scoreDocument = documentMementoConverter.Convert(documentModel.ScoreDocument);
            var template = documentModel.StyleTemplate;
            return (scoreDocument, template);   
        }


        public void SaveDocument(DocumentViewModel documentReader)
        {
            var task = mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
            {
                Title = "Save as...",
                DefaultExtension = ".sin",
                SuggestedFileName = documentReader.Header,
                ShowOverwritePrompt = true,
                FileTypeChoices = [new FilePickerFileType("Sinfonia files") { Patterns = ["*.sin"] }]
            });
            var result = AsyncHelper.RunSync(() => task);
            if (result is null)
            {
                throw new Exception();
            }

            using var stream = AsyncHelper.RunSync(result.OpenWriteAsync);
            var _document = documentMementoConverter.ConvertBack(documentReader.ScoreDocumentCore.GetMemento());
            var documentModel = new DocumentModel()
            {
                ScoreDocument = _document,
                StyleTemplate = documentReader.CanvasViewModel.ScoreDocumentStyle
            };
            JsonSerializer.Serialize(stream, documentModel);
        }
    }
}
