namespace Sinfonia.API
{
    public interface IAddinSettingsManager
    {
        void Register<TValue>(Func<TValue> getValue, Action<TValue> setValue, string description) where TValue : struct, IEquatable<TValue>;

        void RegisterGrouped<TValue>(string groupKey, Func<TValue> getValue, Action<TValue> setValue, string description) where TValue : struct, IEquatable<TValue>;
    }
}
