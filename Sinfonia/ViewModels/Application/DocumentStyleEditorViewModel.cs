using Avalonia.Media;
using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument.StyleTemplates;
using ColorARGB = StudioLaValse.ScoreDocument.StyleTemplates.ColorARGB;


namespace Sinfonia.ViewModels.Application
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


    public class DocumentStyleEditorViewModel : SideBarContentViewModel
    {
        private readonly ScoreDocumentStyleTemplateViewModel scoreDocumentViewModel;
        private readonly PageStyleTemplateViewModel pageViewModel;
        private readonly StaffSystemStyleTemplateViewModel staffSystemViewModel;
        private readonly StaffGroupStyleTemplateViewModel staffGroupViewModel;
        private readonly StaffStyleTemplateViewModel staffViewModel;
        private readonly ScoreMeasureStyleTemplateViewModel scoreMeasureViewModel;
        private readonly InstrumentRibbonStyleTemplateViewModel instrumentRibbonViewModel;
        private readonly InstrumentMeasureStyleTemplateViewModel instrumentMeasureViewModel;
        private readonly MeasureBlockStyleTemplateViewModel measureBlockViewModel;
        private readonly ChordStyleTemplateViewModel chordViewModel;
        private readonly NoteStyleTemplateViewModel noteViewModel;
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

        public override string Header => "Style Template Editor";

        public DocumentStyleEditorViewModel(ScoreDocumentStyleTemplateViewModel scoreDocumentViewModel,
                                            PageStyleTemplateViewModel pageViewModel,
                                            StaffSystemStyleTemplateViewModel staffSystemViewModel,
                                            StaffGroupStyleTemplateViewModel staffGroupViewModel,
                                            StaffStyleTemplateViewModel staffViewModel,
                                            ScoreMeasureStyleTemplateViewModel scoreMeasureViewModel,
                                            InstrumentRibbonStyleTemplateViewModel instrumentRibbonViewModel,
                                            InstrumentMeasureStyleTemplateViewModel instrumentMeasureViewModel,
                                            MeasureBlockStyleTemplateViewModel measureBlockViewModel,
                                            ChordStyleTemplateViewModel chordViewModel,
                                            NoteStyleTemplateViewModel noteViewModel,
                                            ICommandFactory commandFactory,
                                            IScoreStyleTemplateSaveService scoreStyleTemplateSaveService)
        {

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

            Icon = Material.Icons.MaterialIconKind.DocumentSign;
        }

        public void ToggleExpandAll()
        {
            if (Templates.All(t => t.IsExpanded))
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
            if(yaml is null)
            {
                return;
            }
            scoreDocumentViewModel.ScoreDocumentStyle.Apply(yaml);
            scoreDocumentViewModel.Rerender();
            Rebuild();
        }

        public void SaveYaml()
        {
            scoreStyleTemplateSaveService.Save(scoreDocumentViewModel.ScoreDocumentStyle);
        }
    }

    public abstract class BaseStyleTemplateViewModel : PropertyCollectionViewModel
    {
        private readonly DocumentCollectionViewModel documentCollectionViewModel;

        public BaseStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
        {
            this.documentCollectionViewModel = documentCollectionViewModel;
            ScoreDocumentStyle = scoreDocumentStyleTemplate;
        }

        public ScoreDocumentStyleTemplate ScoreDocumentStyle { get; }

        public void Rerender(Method method = Method.Recursive)
        {
            foreach (var document in documentCollectionViewModel.Documents)
            {
                var canvas = document.CanvasViewModel;

                canvas.Invalidator.Invalidate(canvas.ScoreDocument, method: method);
                canvas.Invalidator.RenderChanges();
            }
        }
    }

    public class ScoreDocumentStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header { get; } = "Document Template";

        public ScoreDocumentStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            Properties.Add(new PropertyViewModel<double>(() => ScoreDocumentStyle.Scale, v => { ScoreDocumentStyle.Scale = v; Rerender(); }, "Scale"));
            Properties.Add(new PropertyViewModel<double>(() => ScoreDocumentStyle.HorizontalStaffLineThickness, v => { ScoreDocumentStyle.HorizontalStaffLineThickness = v; Rerender(); }, "Horizontal Line Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => ScoreDocumentStyle.VerticalStaffLineThickness, v => { ScoreDocumentStyle.VerticalStaffLineThickness = v; Rerender(); }, "Vertical Line Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => ScoreDocumentStyle.StemLineThickness, v => { ScoreDocumentStyle.StemLineThickness = v; Rerender(); }, "Stem Line Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => ScoreDocumentStyle.FirstSystemIndent, v => { ScoreDocumentStyle.FirstSystemIndent = v; Rerender(); }, "First System Indent"));
        }
    }

    public class PageStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header { get; } = "Page Template";

        public PageStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.PageStyleTemplate;

            Properties.Add(new PropertyViewModel<int>(() => template.PageHeight, v => { template.PageHeight = v; Rerender(); }, "Page Height"));
            Properties.Add(new PropertyViewModel<int>(() => template.PageWidth, v => { template.PageWidth = v; Rerender(); }, "Page Width"));

            Properties.Add(new PropertyViewModel<double>(() => template.MarginLeft, v => { template.MarginLeft = v; Rerender(); }, "Margin Left"));
            Properties.Add(new PropertyViewModel<double>(() => template.MarginTop, v => { template.MarginTop = v; Rerender(); }, "Margin Top"));
            Properties.Add(new PropertyViewModel<double>(() => template.MarginRight, v => { template.MarginRight = v; Rerender(); }, "Margin Right"));
            Properties.Add(new PropertyViewModel<double>(() => template.MarginBottom, v => { template.MarginBottom = v; Rerender(); }, "Margin Bottom"));

            Properties.Add(new PropertyViewModel<Color>(() => template.PageColor.T(), v => { template.PageColor = v.T(); Rerender(method: Method.Shallow); }, "Page Color"));
            Properties.Add(new PropertyViewModel<Color>(() => template.ForegroundColor.T(), v => { template.ForegroundColor = v.T(); Rerender(method: Method.Deep); }, "Foreground Color"));
        }
    }

    public class StaffSystemStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header => "Staff System Template";


        public StaffSystemStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.StaffSystemStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.DistanceToNext, v => { template.DistanceToNext = v; Rerender(); }, "Margin Bottom"));
        }
    }

    public class StaffGroupStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header => "Staff Group Template";


        public StaffGroupStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.StaffGroupStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.DistanceToNext, v => { template.DistanceToNext = v; Rerender(); }, "Margin Bottom"));
        }
    }

    public class StaffStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header => "Staff Template";


        public StaffStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.StaffStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.DistanceToNext, v => { template.DistanceToNext = v; Rerender(); }, "Margin Bottom"));
        }
    }


    public class ScoreMeasureStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header { get; } = "Score Measure Template";

        public ScoreMeasureStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.ScoreMeasureStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.PaddingLeft, v => { template.PaddingLeft = v; Rerender(); }, "Space Left"));
            Properties.Add(new PropertyViewModel<double>(() => template.PaddingRight, v => { template.PaddingRight = v; Rerender(); }, "Space Right"));
        }
    }

    public class InstrumentRibbonStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header { get; } = "Instrument Ribbon Template";

        public InstrumentRibbonStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {

        }
    }

    public class InstrumentMeasureStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header => "Instrument Measure Template";

        public InstrumentMeasureStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {

        }
    }

    public class MeasureBlockStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header { get; } = "Measure Block Template";

        public MeasureBlockStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.MeasureBlockStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.StemLength, v => { template.StemLength = v; Rerender(); }, "Stem Length"));
            Properties.Add(new PropertyViewModel<double>(() => template.BeamAngle, v => { template.BeamAngle = v; Rerender(); }, "Bracket Angle"));
            Properties.Add(new PropertyViewModel<double>(() => template.BeamThickness, v => { template.BeamThickness = v; Rerender(); }, "Bracket Thickness"));
            Properties.Add(new PropertyViewModel<double>(() => template.BeamSpacing, v => { template.BeamSpacing = v; Rerender(); }, "Bracket Spacing"));
        }
    }

    public class ChordStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header => "Chord Template";

        public ChordStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.ChordStyleTemplate;

            Properties.Add(new PropertyViewModel<double>(() => template.SpaceRight, v => { template.SpaceRight = v; Rerender(); }, "Space Right"));
        }
    }

    public class NoteStyleTemplateViewModel : BaseStyleTemplateViewModel
    {
        public override string Header { get; } = "Note Template";

        public NoteStyleTemplateViewModel(DocumentCollectionViewModel documentCollectionViewModel, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate) : base(documentCollectionViewModel, scoreDocumentStyleTemplate)
        {
            var template = ScoreDocumentStyle.NoteStyleTemplate;
        }
    }
}
