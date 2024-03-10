using MainViewModel = Sinfonia.Views.DocumentStyleEditor.ViewModels.MainViewModel;
using System.Windows;

namespace Sinfonia.Views.DocumentStyleEditor
{
    /// <summary>
    /// Interaction logic for DocumentStyleEditor.xaml
    /// </summary>
    public partial class DocumentStyleEditorView : Window
    {
        public DocumentStyleEditorView(MainViewModel mainViewModel)
        {
            InitializeComponent();

            DataContext = mainViewModel;
        }
    }
}
