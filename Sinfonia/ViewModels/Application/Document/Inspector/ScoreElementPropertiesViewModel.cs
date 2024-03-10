namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public abstract class ScoreElementPropertiesViewModel : PropertyCollectionViewModel
    {
        private readonly IScoreBuilder scoreBuilder;
        private readonly IScoreLayoutProvider scoreLayoutProvider;

        public IEnumerable<IUniqueScoreElement> Elements { get; }

        internal ScoreElementPropertiesViewModel(IEnumerable<IUniqueScoreElement> uniqueScoreElements, IScoreBuilder scoreBuilder, IScoreLayoutProvider scoreLayoutProvider)
        {
            this.scoreBuilder = scoreBuilder;
            this.scoreLayoutProvider = scoreLayoutProvider;

            Elements = uniqueScoreElements;
        }

        public abstract void Rebuild();

        protected PropertyViewModel<TProperty?> Create<TProperty, TLayout, TEntity, TEditor>(IEnumerable<TEntity> entities,
                                                                                             Func<IScoreLayoutProvider, TEntity, TProperty> propertyGetter,
                                                                                             Action<TLayout, TProperty> propertySetter,
                                                                                             string title) where TProperty : IEquatable<TProperty>
                                                                                                           where TEntity : IScoreEntity, IUniqueScoreElement
                                                                                                           where TEditor : IScoreElementEditor, ILayoutEditor<TLayout>
                                                                                                           where TLayout : ILayoutElement<TLayout>
        {
            return new PropertyViewModel<TProperty?>(() =>
            {
                TEntity firstEntity = entities.First();
                TProperty fistValue = propertyGetter(scoreLayoutProvider, firstEntity);
                return !entities.All(m => propertyGetter(scoreLayoutProvider, m).Equals(fistValue)) ? default : fistValue;
            },
            (val) =>
            {
                if (val is null)
                {
                    return;
                }

                _ = scoreBuilder
                    .Edit<TEditor>(entities.Select(e => e.Id), (element) =>
                    {
                        TLayout layout = element.ReadLayout();
                        propertySetter(layout, val);
                        element.ApplyLayout(layout);
                    })
                    .Build();

            },
            title);
        }
    }
}
