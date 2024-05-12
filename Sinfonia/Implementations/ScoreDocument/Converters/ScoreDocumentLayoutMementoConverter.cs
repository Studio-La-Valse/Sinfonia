using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using StudioLaValse.ScoreDocument.Models.Entities;
using ColorARGB = StudioLaValse.ScoreDocument.Models.Classes.ColorARGB;

namespace Sinfonia.Implementations.ScoreDocument.Converters
{
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


        public ClefChange Convert(StudioLaValse.ScoreDocument.Models.Classes.ClefChange clefChange)
        {
            var _clefChange = new ClefChange(elementConverter.Convert(clefChange.Clef), clefChange.StaffIndex, elementConverter.Convert(clefChange.Position));
            return _clefChange;
        }
        public StudioLaValse.ScoreDocument.Models.Classes.ClefChange ConvertBack(ClefChange clefChange)
        {
            var _clefChange = new StudioLaValse.ScoreDocument.Models.Classes.ClefChange()
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
}
