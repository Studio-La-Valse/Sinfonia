namespace Sinfonia.API
{
    public interface IAddinSettingsManager
    {
        void Register<T>(Func<T> getValue, Action<T> setValue, string description);
    }
}
