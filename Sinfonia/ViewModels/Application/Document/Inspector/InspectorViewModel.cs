namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class InspectorViewModel : BaseViewModel, IObserver<IUniqueScoreElement>
    {
        private readonly HashSet<IUniqueScoreElement> selectedElements = new(new KeyEqualityComparer<IUniqueScoreElement, int>(e => e.Id));
        private readonly IScoreBuilder scoreBuilder;
        private readonly IScoreLayoutProvider scoreLayoutDictionary;

        public ScoreElementPropertiesViewModel? PropertiesViewModel
        {
            get => GetValue(() => PropertiesViewModel);
            set => SetValue(() => PropertiesViewModel, value);
        }

        internal InspectorViewModel(IScoreBuilder scoreBuilder, IScoreLayoutProvider scoreLayoutDictionary)
        {
            this.scoreBuilder = scoreBuilder;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
        }

        public void Update(IEnumerable<IUniqueScoreElement> selected, IEnumerable<IUniqueScoreElement> unselected)
        {
            foreach (IUniqueScoreElement item in unselected)
            {
                _ = selectedElements.Remove(item);
            }

            foreach (IUniqueScoreElement item in selected)
            {
                _ = selectedElements.Add(item);
            }

            Update();
        }

        private void Update()
        {
            if (selectedElements.Count == 0)
            {
                PropertiesViewModel = null;
                return;
            }

            IUniqueScoreElement firstItem = selectedElements.Last();
            Type firstType = firstItem.GetType();
            if (!selectedElements.All(e => e.GetType().Name == firstType.Name))
            {
                PropertiesViewModel = null;
                return;
            }

            PropertiesViewModel = firstItem switch
            {
                INoteReader _ => new NotePropertiesViewModel(selectedElements.OfType<INoteReader>(), scoreBuilder, scoreLayoutDictionary),
                _ => null
            };

            PropertiesViewModel?.Rebuild();
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

            if (PropertiesViewModel is null)
            {
                return;
            }

            if (PropertiesViewModel.Elements.Any(e => e.Id.Equals(value.Id)))
            {
                refreshOnNextInvalidation = true;
            }
        }
    }
}
