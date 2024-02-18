using System.Windows;

namespace Sinfonia.Implementations
{
    internal class ShellMethods : IShellMethods
    {
        public bool Confirm(string message)
        {
            return true;
        }

        public void Exit()
        {
            Application.Current.Shutdown();
        }
    }
}
