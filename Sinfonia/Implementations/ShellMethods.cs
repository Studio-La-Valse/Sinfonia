using Sinfonia.Windows;
using System.Windows;

namespace Sinfonia.Implementations
{
    internal class ShellMethods : IShellMethods
    {
        private readonly MainWindow mainWindow;

        public ShellMethods(MainWindow mainWindow)
        {
            this.mainWindow = mainWindow;
        }

        public void Exit()
        {
            mainWindow.Close();
        }
    }
}
