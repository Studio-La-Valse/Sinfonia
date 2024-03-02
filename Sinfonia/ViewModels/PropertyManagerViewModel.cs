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

        protected static PropertyViewModel<TProperty?> Create<TProperty, TLayout, TEntity, TEditor>(IEnumerable<TEntity> entities, 
                                                                                                    Func<TEntity, TProperty> propertyGetter, 
                                                                                                    Func<IScoreLayoutBuilder, TEditor, TLayout> layoutGetter, 
                                                                                                    Action<TLayout, TProperty> propertySetter,
                                                                                                    Action<IScoreLayoutBuilder, TLayout, TEditor> layoutSetter,
                                                                                                    IScoreBuilder scoreBuilder, 
                                                                                                    string title) where TProperty : IEquatable<TProperty>
                                                                                                                  where TEntity : IUniqueScoreElement
                                                                                                                  where TEditor : IScoreElementEditor
        {
            return new PropertyViewModel<TProperty?>(() =>
            {
                var firstEntity = entities.First();
                var fistValue = propertyGetter(firstEntity);
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
                    .Edit<TEditor>(entities.Select(e => e.Id), (element, layoutBuilder) =>
                    {
                        var layout = layoutGetter(layoutBuilder, element);
                        propertySetter(layout, val);
                        layoutSetter(layoutBuilder, layout, element);
                    })
                    .Build();

            },
            title);
        }
    }
}
