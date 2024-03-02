namespace Sinfonia.ViewModels.Application.Document.Explorer
{
    public class ScoreElementViewModel : BaseViewModel
    {
        private readonly IScoreElement uniqueScoreElement;

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

        public IScoreElement UniqueScoreElement => uniqueScoreElement;

        public ScoreElementViewModel(IScoreElement uniqueScoreElement)
        {
            this.uniqueScoreElement = uniqueScoreElement;

            ScoreElements = [];

            Name = uniqueScoreElement.ToString() ?? "Unnamed element.";
        }

        public void Rebuild()
        {
            ScoreElements.Clear();

            foreach (var child in uniqueScoreElement.EnumerateChildren())
            {
                var vm = new ScoreElementViewModel(child);
                vm.Rebuild();
                ScoreElements.Add(vm);
            }
        }
    }
}
