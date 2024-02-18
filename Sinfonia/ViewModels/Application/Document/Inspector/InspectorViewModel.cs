﻿namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public class InspectorViewModel : BaseViewModel, IObserver<IUniqueScoreElement>
    {
        private readonly HashSet<IUniqueScoreElement> selectedElements = [];
        private readonly IScoreBuilder scoreBuilder;

        public ScoreElementPropertiesViewModel? PropertiesViewModel
        {
            get => GetValue(() => PropertiesViewModel);
            set => SetValue(() => PropertiesViewModel, value);
        }

        internal InspectorViewModel(IScoreBuilder scoreBuilder)
        {
            this.scoreBuilder = scoreBuilder;
        }

        public void Update(IEnumerable<IUniqueScoreElement> selected, IEnumerable<IUniqueScoreElement> unselected)
        {
            foreach (var item in unselected)
            {
                selectedElements.Remove(item);
            }

            foreach (var item in selected)
            {
                selectedElements.Add(item);
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

            var firstItem = selectedElements.Last();
            var firstType = firstItem.GetType();
            if (!selectedElements.All(e => e.GetType().Name == firstType.Name))
            {
                PropertiesViewModel = null;
                return;
            }

            PropertiesViewModel = firstItem switch
            {
                INoteEditor _ => new NotePropertiesViewModel(selectedElements.OfType<INoteEditor>(), scoreBuilder),
                _ => null
            };

            if (PropertiesViewModel is not null)
            {
                PropertiesViewModel.Rebuild();
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

            if (PropertiesViewModel is null)
            {
                return;
            }

            if (PropertiesViewModel.Elements.Any(e => e.Equals(value)))
            {
                refreshOnNextInvalidation = true;
            }
        }
    }
}