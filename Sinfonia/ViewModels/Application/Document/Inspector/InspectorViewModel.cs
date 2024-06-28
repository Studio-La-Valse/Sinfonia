using Sinfonia.ViewModels.Base;
using StudioLaValse.ScoreDocument;

namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class InspectorViewModel : BaseViewModel, IObserver<IUniqueScoreElement>
    {
        private readonly HashSet<IUniqueScoreElement> selectedElements = new(new KeyEqualityComparer<IUniqueScoreElement, int>(e => e.Id));
        private readonly IScoreBuilder scoreBuilder;

        public ObservableCollection<PropertyCollectionViewModel> PropertiesViewModel
        {
            get => GetValue(() => PropertiesViewModel);
            set => SetValue(() => PropertiesViewModel, value);
        }

        public ICommand ToggleExpandAllCommand
        {
            get => GetValue(() => ToggleExpandAllCommand);
            set => SetValue(() => ToggleExpandAllCommand, value);
        }

        public InspectorViewModel(IScoreBuilder scoreBuilder, ICommandFactory commandFactory)
        {
            this.scoreBuilder = scoreBuilder;

            PropertiesViewModel = [];
            ToggleExpandAllCommand = commandFactory.Create(ToggleExpandAll);
        }

        public void ToggleExpandAll()
        {
            if(PropertiesViewModel.All(p => p.IsExpanded))
            {
                PropertiesViewModel.ForEach(p => p.IsExpanded = false);
                return;
            }

            PropertiesViewModel.ForEach(p => p.IsExpanded= true);
        }

        public void Update(IEnumerable<IUniqueScoreElement> selected, IEnumerable<IUniqueScoreElement> unselected)
        {
            foreach (var item in unselected)
            {
                _ = selectedElements.Remove(item);
            }

            foreach (var item in selected)
            {
                _ = selectedElements.Add(item);
            }

            Update();
        }

        private void Update()
        {
            PropertiesViewModel.Clear();

            if (selectedElements.Count == 0)
            {
                return;
            }

            var notes = selectedElements.OfType<INote>();
            if (notes.Any())
            {
                PropertiesViewModel.Add(new NotePropertiesViewModel(selectedElements.OfType<INote>(), scoreBuilder));
            }

            var chords = selectedElements.OfType<IChord>();
            if (chords.Any())
            {
                PropertiesViewModel.Add(new ChordPropertiesViewModel(selectedElements.OfType<IChord>(), scoreBuilder));
            }

            var blocks = selectedElements.OfType<IMeasureBlock>();
            if (blocks.Any())
            {
                PropertiesViewModel.Add(new MeasureBlockPropertiesViewModel(selectedElements.OfType<IMeasureBlock>(), scoreBuilder));
            }

            var instrumentMeasures = selectedElements.OfType<IInstrumentMeasure>();
            if (instrumentMeasures.Any())
            {
                PropertiesViewModel.Add(new InstrumentMeasurePropertiesViewModel(selectedElements.OfType<IInstrumentMeasure>(), scoreBuilder));
            }

            var scoreMeasures = selectedElements.OfType<IScoreMeasure>();
            if (scoreMeasures.Any())
            {
                PropertiesViewModel.Add(new ScoreMeasurePropertiesViewModel(selectedElements.OfType<IScoreMeasure>(), scoreBuilder));
            }

            var ribbons = selectedElements.OfType<IInstrumentRibbon>();
            if (ribbons.Any())
            {
                PropertiesViewModel.Add(new InstrumentRibbonPropertiesViewModel(selectedElements.OfType<IInstrumentRibbon>(), scoreBuilder));
            }

            var scores = selectedElements.OfType<IScoreDocument>();
            if (scores.Any())
            {
                PropertiesViewModel.Add(new ScoreDocumentPropertiesViewModel(selectedElements.OfType<IScoreDocument>(), scoreBuilder));
            }
        }

        private bool refreshOnNextInvalidation = false;
        public void OnCompleted()
        {
            if (!refreshOnNextInvalidation)
            {
                return;
            }

            Update();
            refreshOnNextInvalidation = false;
        }

        public void OnError(Exception error)
        {
            throw error;
        }

        public void OnNext(IUniqueScoreElement value)
        {
            if (refreshOnNextInvalidation)
            {
                return;
            }

            if (PropertiesViewModel.Count == 0)
            {
                return;
            }

            if (selectedElements.Any(e => e.Id.Equals(value.Id)))
            {
                refreshOnNextInvalidation = true;
            }
        }
    }
}
