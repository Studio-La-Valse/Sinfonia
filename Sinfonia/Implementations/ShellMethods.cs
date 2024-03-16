using System.Windows;

namespace Sinfonia.Implementations
{
    internal class ShellMethods : IShellMethods
    {
        public void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
