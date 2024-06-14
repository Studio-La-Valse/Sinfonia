using Avalonia;
using Avalonia.Markup.Xaml.Templates;
using Avalonia.Media;
using Sinfonia.ViewModels.Base;
using System.IO;
using ColorARGB = StudioLaValse.ScoreDocument.Layout.Templates.ColorARGB;


namespace Sinfonia.ViewModels.Application.Document.StyleTemplate
{
    public static class ColorExtensions
    {
        public static Color T(this ColorARGB color)
        {
            return new Color((byte)color.A, (byte)color.R, (byte)color.G, (byte)color.B);
        }

        public static ColorARGB T(this Color color)
        {
            return new ColorARGB()
            {
                A = color.A,
                R = color.R,
                G = color.G,
                B = color.B,
            };
        }
    }


    public class DocumentStyleEditorViewModel : BaseViewModel
    {
        private readonly CanvasViewModel canvasViewModel;
        private readonly ScoreDocumentViewModel scoreDocumentViewModel;
        private readonly PageViewModel pageViewModel;
        private readonly StaffSystemViewModel staffSystemViewModel;
        private readonly StaffGroupViewModel staffGroupViewModel;
        private readonly StaffViewModel staffViewModel;
        private readonly ScoreMeasureViewModel scoreMeasureViewModel;
        private readonly InstrumentRibbonViewModel instrumentRibbonViewModel;
        private readonly InstrumentMeasureViewModel instrumentMeasureViewModel;
        private readonly MeasureBlockViewModel measureBlockViewModel;
        private readonly ChordViewModel chordViewModel;
        private readonly NoteViewModel noteViewModel;
        private readonly IScoreStyleTemplateSaveService scoreStyleTemplateSaveService;

        public ObservableCollection<PropertyCollectionViewModel> Templates
        {
            get => GetValue(() => Templates);
            set => SetValue(() => Templates, value);
        }

        public ICommand LoadYamlCommand
        {
            get => GetValue(() => LoadYamlCommand);
            set => SetValue(() => LoadYamlCommand, value);
        }

        public ICommand SaveYamlCommand
        {
            get => GetValue(() => SaveYamlCommand);
            set => SetValue(() => SaveYamlCommand, value);
        }

        public ICommand ToggleExpandAllCommand
        {
            get => GetValue(() => ToggleExpandAllCommand);
            set => SetValue(() => ToggleExpandAllCommand, value);
        }

        public DocumentStyleEditorViewModel(CanvasViewModel canvasViewModel,
                                            ScoreDocumentViewModel scoreDocumentViewModel,
                                            PageViewModel pageViewModel,
                                            StaffSystemViewModel staffSystemViewModel,
                                            StaffGroupViewModel staffGroupViewModel,
                                            StaffViewModel staffViewModel,
                                            ScoreMeasureViewModel scoreMeasureViewModel,
                                            InstrumentRibbonViewModel instrumentRibbonViewModel,
                                            InstrumentMeasureViewModel instrumentMeasureViewModel,
                                            MeasureBlockViewModel measureBlockViewModel,
                                            ChordViewModel chordViewModel,
                                            NoteViewModel noteViewModel,
                                            ICommandFactory commandFactory,
                                            IScoreStyleTemplateSaveService scoreStyleTemplateSaveService)
        {

            this.canvasViewModel = canvasViewModel;
            this.scoreDocumentViewModel = scoreDocumentViewModel;
            this.pageViewModel = pageViewModel;
            this.staffSystemViewModel = staffSystemViewModel;
            this.staffGroupViewModel = staffGroupViewModel;
            this.staffViewModel = staffViewModel;
            this.scoreMeasureViewModel = scoreMeasureViewModel;
            this.instrumentRibbonViewModel = instrumentRibbonViewModel;
            this.instrumentMeasureViewModel = instrumentMeasureViewModel;
            this.measureBlockViewModel = measureBlockViewModel;
            this.chordViewModel = chordViewModel;
            this.noteViewModel = noteViewModel;
            this.scoreStyleTemplateSaveService = scoreStyleTemplateSaveService;
            Templates = [];
            SaveYamlCommand = commandFactory.Create(SaveYaml);
            LoadYamlCommand = commandFactory.Create(LoadYaml);
            ToggleExpandAllCommand = commandFactory.Create(ToggleExpandAll);

            Rebuild();
        }

        public void ToggleExpandAll()
        {
            if(Templates.All(t => t.IsExpanded))
            {
                Templates.ForEach(t => t.IsExpanded = false);
                return;
            }

            Templates.ForEach(t => t.IsExpanded = true);
        }

        public void Rebuild()
        {
            Templates.Clear();
            Templates.Add(scoreDocumentViewModel);
            Templates.Add(pageViewModel);
            Templates.Add(staffSystemViewModel);
            Templates.Add(staffGroupViewModel);
            Templates.Add(staffViewModel);
            Templates.Add(instrumentRibbonViewModel);
            Templates.Add(scoreMeasureViewModel);
            Templates.Add(instrumentMeasureViewModel);
            Templates.Add(measureBlockViewModel);
            Templates.Add(chordViewModel);
            Templates.Add(noteViewModel);
        }
        public void LoadYaml()
        {
            var yaml = scoreStyleTemplateSaveService.Open();
            canvasViewModel.ScoreDocumentStyle.Apply(yaml);
            canvasViewModel.Rerender();
            Rebuild();
        }

        public void SaveYaml()
        {
            scoreStyleTemplateSaveService.Save(canvasViewModel.ScoreDocumentStyle);
        }
    }

    public class ScoreDocumentViewModel : PropertyCollectionViewModel
    {
        public override string Header { get; } = "Document Template";

        public ScoreDocumentViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle;

            Properties.Add(new PropertyViewModel<double>(() => template.Scale, v => { template.Scale = v; canvasViewModel.Rerender(); }, "Scale"));
            Properties.Add(new PropertyViewModel<double>(() => template.HorizontalStaffLineThickness, v => { template.HorizontalStaffLineThickness = v; canvasViewModel.Rerender(); }, "Horizontal Line Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => template.VerticalStaffLineThickness, v => { template.VerticalStaffLineThickness = v; canvasViewModel.Rerender(); }, "Vertical Line Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => template.StemLineThickness, v => { template.StemLineThickness = v; canvasViewModel.Rerender(); }, "Stem Line Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => template.FirstSystemIndent, v => { template.FirstSystemIndent = v; canvasViewModel.Rerender(); }, "First System Indent"));
            Properties.Add(new PropertyViewModel<double>(() => template.ChordPositionFactor, v => { template.ChordPositionFactor = v; canvasViewModel.Rerender(); }, "Chord Position Factor"));
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

            Properties.Add(new PropertyViewModel<Color>(
                () => template.PageColor.T(), 
                v => 
                { 
                    template.PageColor = v.T(); 
                    canvasViewModel.Invalidator.Invalidate(canvasViewModel.ScoreDocumentReader, method:Method.Shallow);
                    canvasViewModel.Invalidator.RenderChanges();
                }, 
                "Page Color"));
            Properties.Add(new PropertyViewModel<Color>(
                () => template.ForegroundColor.T(), 
                v => 
                { 
                    template.ForegroundColor = v.T();
                    canvasViewModel.Invalidator.Invalidate(canvasViewModel.ScoreDocumentReader, method: Method.Deep);
                    canvasViewModel.Invalidator.RenderChanges();
                }, 
                "Foreground Color"));
        }
    }

    public class StaffSystemViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Staff System Template";


        public StaffSystemViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.StaffSystemStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.DistanceToNext, v => { template.DistanceToNext = v; canvasViewModel.Rerender(); }, "Margin Bottom"));
        }
    }

    public class StaffGroupViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Staff Group Template";


        public StaffGroupViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.StaffGroupStyleTemplate;

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

            Properties.Add(new PropertyViewModel<double>(() => template.PaddingLeft, v => { template.PaddingLeft = v; canvasViewModel.Rerender(); }, "Space Left"));
            Properties.Add(new PropertyViewModel<double>(() => template.PaddingRight, v => { template.PaddingRight = v; canvasViewModel.Rerender(); }, "Space Right"));
        }
    }

    public class InstrumentRibbonViewModel : PropertyCollectionViewModel
    {
        public override string Header { get; } = "Instrument Ribbon Template";

        public InstrumentRibbonViewModel(CanvasViewModel canvasViewModel)
        {

        }
    }

    public class InstrumentMeasureViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Instrument Measure Template";

        public InstrumentMeasureViewModel(CanvasViewModel canvasViewModel)
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
            Properties.Add(new PropertyViewModel<double>(() => template.BeamAngle, v => { template.BeamAngle = v; canvasViewModel.Rerender(); }, "Bracket Angle"));
            Properties.Add(new PropertyViewModel<double>(() => template.BeamThickness, v => { template.BeamThickness = v; canvasViewModel.Rerender(); }, "Bracket Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => template.BeamSpacing, v => { template.BeamSpacing = v; canvasViewModel.Rerender(); }, "Bracket Spacing"));
        }
    }

    public class ChordViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Chord Template";

        public ChordViewModel(CanvasViewModel canvasViewModel)
        {
            var template = canvasViewModel.ScoreDocumentStyle.ChordStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.SpaceRight, v => { template.SpaceRight = v; canvasViewModel.Rerender(); }, "Space Right"));
        }
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
