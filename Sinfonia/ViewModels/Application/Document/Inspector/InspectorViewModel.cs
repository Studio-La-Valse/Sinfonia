namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class InspectorViewModel : BaseViewModel, IObserver<IUniqueScoreElement>
    {
        private readonly HashSet<IUniqueScoreElement> selectedElements = new(new KeyEqualityComparer<IUniqueScoreElement, int>(e => e.Id));
        private readonly IScoreBuilder scoreBuilder;
        private readonly IScoreDocumentLayout scoreLayoutDictionary;

        public PropertyCollectionViewModel? PropertiesViewModel
        {
            get => GetValue(() => PropertiesViewModel);
            set => SetValue(() => PropertiesViewModel, value);
        }

        internal InspectorViewModel(IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutDictionary)
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
                IChordReader _ => new ChordPropertiesViewModel(selectedElements.OfType<IChordReader>(), scoreBuilder, scoreLayoutDictionary),
                IMeasureBlockReader _ => new MeasureBlockPropertiesViewModel(selectedElements.OfType<IMeasureBlockReader>(), scoreBuilder, scoreLayoutDictionary),
                IInstrumentMeasureReader _ => new InstrumentMeasurePropertiesViewModel(selectedElements.OfType<IInstrumentMeasureReader>(), scoreBuilder, scoreLayoutDictionary),
                IScoreMeasureReader _ => new ScoreMeasurePropertiesViewModel(selectedElements.OfType<IScoreMeasureReader>(), scoreBuilder, scoreLayoutDictionary),
                IInstrumentRibbonReader _ => new InstrumentRibbonPropertiesViewModel(selectedElements.OfType<IInstrumentRibbonReader>(), scoreBuilder, scoreLayoutDictionary),
                IStaffReader _ => new StaffPropertiesViewModel(selectedElements.OfType<IStaffReader>(), scoreBuilder, scoreLayoutDictionary),
                IStaffGroupReader _ => new StaffGroupPropertiesViewModel(selectedElements.OfType<IStaffGroupReader>(), scoreBuilder, scoreLayoutDictionary),
                IStaffSystemReader _ => new StaffSystemPropertiesViewModel(selectedElements.OfType<IStaffSystemReader>(), scoreBuilder, scoreLayoutDictionary),
                IPageReader _ => new PagePropertiesViewModel(selectedElements.OfType<IPageReader>(), scoreBuilder, scoreLayoutDictionary),
                IScoreDocumentReader _ => new ScoreDocumentPropertiesViewModel(selectedElements.OfType<IScoreDocumentReader>(), scoreBuilder, scoreLayoutDictionary),
                _ => null
            };
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

            if (selectedElements.Any(e => e.Id.Equals(value.Id)))
            {
                refreshOnNextInvalidation = true;
            }
        }
    }
}
