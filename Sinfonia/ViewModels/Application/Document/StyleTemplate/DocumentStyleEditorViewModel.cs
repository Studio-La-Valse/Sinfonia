using StudioLaValse.ScoreDocument.Layout.Templates;
using System.IO;
using YamlDotNet.Serialization;
using IBrowseToFile = Sinfonia.Interfaces.IBrowseToFile;

namespace Sinfonia.ViewModels.Application.Document.StyleTemplate
{
    public class DocumentStyleEditorViewModel : BaseViewModel
    {
        private readonly CanvasViewModel canvasViewModel;
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
        private readonly ICommandFactory commandFactory;
        private readonly IYamlConverter yamlConverter;
        private readonly IBrowseToFile browseToFile;
        private readonly ISaveFile saveFile;

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

        public DocumentStyleEditorViewModel(CanvasViewModel canvasViewModel,
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
                                            IYamlConverter yamlConverter,
                                            IBrowseToFile browseToFile,
                                            ISaveFile saveFile)
        {
            
            this.canvasViewModel = canvasViewModel;
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
            this.commandFactory = commandFactory;
            this.yamlConverter = yamlConverter;
            this.browseToFile = browseToFile;
            this.saveFile = saveFile;
            Templates = [];
            SaveYamlCommand = commandFactory.Create(SaveYaml);
            LoadYamlCommand = commandFactory.Create(LoadYaml);
        }

        public void Rebuild()
        {
            Templates.Clear();
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
            if(browseToFile.BrowseToFile(".yaml", "Yaml Files(*.yaml)|*.yaml|Yaml Files(*.yml)|*.yml", out var filePath))
            {
                using var reader = File.OpenText(filePath);
                var yaml = yamlConverter.FromYaml(reader);
                canvasViewModel.ScoreDocumentStyle.Apply(yaml);
                canvasViewModel.Rerender();
                Rebuild();
            }
        }

        public void SaveYaml()
        {
            if (saveFile.SaveToFile("my_style", ".yaml", "Yaml Files(*.yaml)|*.yaml|Yaml Files(*.yml)|*.yml", out var filePath))
            {
                var yaml = yamlConverter.ToYaml(canvasViewModel.ScoreDocumentStyle);
                using var writer = File.CreateText(filePath);
                writer.Write(yaml);
            }
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
            Properties.Add(new PropertyViewModel<double>(() => template.BracketAngle, v => { template.BracketAngle = v; canvasViewModel.Rerender(); }, "Bracket Angle"));
        }
    }

    public class ChordViewModel : PropertyCollectionViewModel
    {
        public override string Header => "Chord Template";

        public ChordViewModel(CanvasViewModel canvasViewModel)
        {

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
