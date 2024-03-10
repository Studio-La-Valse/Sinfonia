namespace Sinfonia.ViewModels.Application.Document.Explorer
{
    public class ScoreElementViewModel : BaseViewModel
    {
        public string Name
        {
            get => GetValue(() => Name);
            set => SetValue(() => Name, value);
        }
        public ObservableCollection<ScoreElementViewModel> ScoreElements
        {
            get => GetValue(() => ScoreElements);
            set => SetValue(() => ScoreElements, value);
        }

        public IScoreElement UniqueScoreElement { get; }

        public ScoreElementViewModel(IScoreElement uniqueScoreElement)
        {
            UniqueScoreElement = uniqueScoreElement;

            ScoreElements = [];

            Name = uniqueScoreElement.ToString() ?? "Unnamed element.";
        }

        public void Rebuild()
        {
            ScoreElements.Clear();

            foreach (IScoreElement child in UniqueScoreElement.EnumerateChildren())
            {
                ScoreElementViewModel vm = new(child);
                vm.Rebuild();
                ScoreElements.Add(vm);
            }
        }
    }
}
