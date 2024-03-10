using Sinfonia.ViewModels.Application.Document;

namespace Sinfonia.Views.DocumentStyleEditor.ViewModels
{
    public class DocumentStyleEditorViewModel : BaseViewModel
    {
        public ObservableCollection<PropertyCollectionViewModel> Templates
        {
            get => GetValue(() => Templates);
            set => SetValue(() => Templates, value);
        }

        public DocumentStyleEditorViewModel(PageViewModel pageViewModel,
                                            StaffSystemViewModel staffSystemViewModel,
                                            StaffGroupViewModel staffGroupViewModel,
                                            StaffViewModel staffViewModel,
                                            ScoreMeasureViewModel scoreMeasureViewModel, 
                                            InstrumentRibbonViewModel instrumentRibbonViewModel,
                                            InstrumentMeasureViewModel instrumentMeasureViewModel,
                                            MeasureBlockViewModel measureBlockViewModel,
                                            ChordViewModel chordViewModel,
                                            NoteViewModel noteViewModel)
        {
            Templates = 
            [
                pageViewModel,
                staffSystemViewModel,
                staffGroupViewModel,
                staffViewModel,
                instrumentRibbonViewModel,
                scoreMeasureViewModel,
                instrumentMeasureViewModel,
                measureBlockViewModel,
                chordViewModel,
                noteViewModel
            ];
        }


        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Templates));
        }
    }

    public class PageViewModel : PropertyCollectionViewModel
    {
        public override string Header { get; } = "Page Template";

        public PageViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.PageStyleTemplate;

            Properties.Add(new PropertyViewModel<int>(() => template.PageHeight, v => { template.PageHeight = v; canvasViewModel.Rerender(); }, "Page Height"));
            Properties.Add(new PropertyViewModel<int>(() => template.PageWidth, v => { template.PageWidth = v; canvasViewModel.Rerender(); }, "Page Width"));

            Properties.Add(new PropertyViewModel<double>(() => template.MarginLeft, v => { template.MarginLeft = v; canvasViewModel.Rerender(); }, "Margin Left"));
            Properties.Add(new PropertyViewModel<double>(() => template.MarginTop, v => { template.MarginTop = v; canvasViewModel.Rerender(); }, "Margin Top"));
            Properties.Add(new PropertyViewModel<double>(() => template.MarginRight, v => { template.MarginRight = v; canvasViewModel.Rerender(); }, "Margin Right"));
            Properties.Add(new PropertyViewModel<double>(() => template.MarginBottom, v => { template.MarginBottom = v; canvasViewModel.Rerender(); }, "Margin Bottom"));
        }
    }

    public class StaffSystemViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Staff System Template";


        public StaffSystemViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.StaffSystemStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.PaddingBottom, v => { template.PaddingBottom = v; canvasViewModel.Rerender(); }, "Margin Bottom"));
        }
    }

    public class StaffGroupViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Staff Group Template";


        public StaffGroupViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.StaffGroupStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.LineSpacing, v => { template.LineSpacing = v; canvasViewModel.Rerender(); }, "Line Spacing"));
            Properties.Add(new PropertyViewModel<double>(() => template.DistanceToNext, v => { template.DistanceToNext = v; canvasViewModel.Rerender(); }, "Margin Bottom"));
        }
    }

    public class StaffViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Staff Template";


        public StaffViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.StaffStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.DistanceToNext, v => { template.DistanceToNext = v; canvasViewModel.Rerender(); }, "Margin Bottom"));
        }
    }


    public class ScoreMeasureViewModel : PropertyCollectionViewModel
    {
        public override string Header { get; } = "Score Measure Template";

        public ScoreMeasureViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.ScoreMeasureStyleTemplate;

            Properties.Add(new PropertyViewModel<int>(() => template.Width, v => { template.Width = v; canvasViewModel.Rerender(); }, "Width"));
            Properties.Add(new PropertyViewModel<int>(() => template.PaddingLeft, v => { template.PaddingLeft = v; canvasViewModel.Rerender(); }, "Space Left"));
            Properties.Add(new PropertyViewModel<int>(() => template.PaddingRight, v => { template.PaddingRight = v; canvasViewModel.Rerender(); }, "Space Right"));
        }
    }

    public class InstrumentRibbonViewModel : PropertyCollectionViewModel
    {
        public override string Header { get; } = "Instrument Ribbon Template";

        public InstrumentRibbonViewModel()
        {
            
        }
    }

    public class InstrumentMeasureViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Instrument Measure Template";

        public InstrumentMeasureViewModel()
        {
            
        }
    }

    public class MeasureBlockViewModel : PropertyCollectionViewModel
    {
        public override string Header { get; } = "Measure Block Template";

        public MeasureBlockViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.MeasureBlockStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.StemLength, v => { template.StemLength = v; canvasViewModel.Rerender(); }, "Stem Length"));
            Properties.Add(new PropertyViewModel<double>(() => template.BracketAngle, v => { template.BracketAngle = v; canvasViewModel.Rerender(); }, "Bracket Angle"));
        }
    }

    public class ChordViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Chord Template";
    }

    public class NoteViewModel : PropertyCollectionViewModel
    {
        public override string Header { get; } = "Note Template";

        public NoteViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.NoteStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.Scale, v => { template.Scale = v; canvasViewModel.Rerender(); }, "Scale"));
            Properties.Add(new PropertyViewModel<AccidentalDisplay>(() => template.AccidentalDisplay, v => { template.AccidentalDisplay = v; canvasViewModel.Rerender(); }, "Accidentals"));
        }
    }
}
