namespace Sinfonia.ViewModels
{
    public class PropertyManagerViewModel : BaseViewModel
    {
        public ObservableCollection<PropertyViewModel> Properties
        {
            get => GetValue(() => Properties);
            set => SetValue(() => Properties, value);
        }

        public PropertyManagerViewModel()
        {
            Properties = [];
        }

        protected static PropertyViewModel<TProperty?> Create<TProperty, TEntity>(IEnumerable<TEntity> entities, Func<TEntity, TProperty> propertyGetter, Action<TEntity, TProperty> propertySetter, string title) where TProperty : IEquatable<TProperty>
        {
            return new PropertyViewModel<TProperty?>(() =>
            {
                var fistValue = propertyGetter(entities.First());
                if (!entities.All(m => propertyGetter(m).Equals(fistValue)))
                {
                    return default;
                }

                return fistValue;
            },
            (val) =>
            {
                if (val is null)
                {
                    return;
                }

                foreach (var entity in entities)
                {
                    propertySetter(entity, val);
                }
            },
            title);
        }

        protected static PropertyViewModel<TProperty?> Create<TProperty, TEntity>(IEnumerable<TEntity> entities, Func<TEntity, TProperty> propertyGetter, Action<TProperty> propertySetter, string title) where TProperty : IEquatable<TProperty>
        {
            return new PropertyViewModel<TProperty?>(() =>
            {
                var fistValue = propertyGetter(entities.First());
                if (!entities.All(m => propertyGetter(m).Equals(fistValue)))
                {
                    return default;
                }

                return fistValue;
            },
            (val) =>
            {
                if (val is null)
                {
                    return;
                }

                propertySetter(val);
            },
            title);
        }

        protected static PropertyViewModel<TProperty?> Create<TProperty, TEntity>(IEnumerable<TEntity> entities, Func<TEntity, TProperty> propertyGetter, Action<TEntity, TProperty> propertySetter, IScoreBuilder scoreBuilder, string title) where TProperty : IEquatable<TProperty>
        {
            return new PropertyViewModel<TProperty?>(() =>
            {
                var fistValue = propertyGetter(entities.First());
                if (!entities.All(m => propertyGetter(m).Equals(fistValue)))
                {
                    return default;
                }

                return fistValue;
            },
            (val) =>
            {
                if (val is null)
                {
                    return;
                }

                scoreBuilder
                    .Edit(_ =>
                    {
                        foreach (var entity in entities)
                        {
                            propertySetter(entity, val);
                        }
                    })
                    .Build();
                
            },
            title);
        }
    }
}
