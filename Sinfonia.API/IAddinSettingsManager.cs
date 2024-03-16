using StudioLaValse.ScoreDocument.Core.Primitives;
using StudioLaValse.ScoreDocument.Layout;

namespace Sinfonia.API
{
    public interface IAddinSettingsManager
    {
        void Register<TValue>(Func<TValue> getValue, Action<TValue> setValue, string description) where TValue : struct, IEquatable<TValue>;

        void RegisterGrouped<TValue>(string groupKey, Func<TValue> getValue, Action<TValue> setValue, string description) where TValue : struct, IEquatable<TValue>;

        void RegisterElementLayout<TElement, TLayout>(string description) where TElement : IScoreElement, IScoreEntity where TLayout : class, ILayoutElement<TLayout>;

        void RegisterLayoutProperty<TLayout, TProperty>(string key, string description) where TLayout : class, ILayoutElement<TLayout> where TProperty : struct, IEquatable<TProperty>;
    }
}
