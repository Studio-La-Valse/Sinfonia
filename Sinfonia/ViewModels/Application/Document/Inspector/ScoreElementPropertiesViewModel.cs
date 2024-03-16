namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public abstract class ScoreElementPropertiesViewModel<TEntity, TEditor, TLayout> : PropertyCollectionViewModel
            where TEntity : IScoreEntity, IUniqueScoreElement
            where TEditor : IScoreElementEditor, ILayoutEditor<TLayout>
            where TLayout : class, ILayoutElement<TLayout>
    {
        private readonly IScoreBuilder scoreBuilder;
        private readonly IScoreDocumentLayout scoreLayoutProvider;
        private readonly IEnumerable<TEntity> notes;

        internal ScoreElementPropertiesViewModel(IScoreBuilder scoreBuilder, IScoreDocumentLayout scoreLayoutProvider, IEnumerable<TEntity> notes)
        {
            this.scoreBuilder = scoreBuilder;
            this.scoreLayoutProvider = scoreLayoutProvider;
            this.notes = notes;
        }

        public abstract TLayout GetLayout(IScoreDocumentLayout scoreLayoutProvider, TEntity entity);

        protected PropertyViewModel<TProperty> Create<TProperty>(Func<TLayout, TProperty> propertyGetter, Action<TLayout, TProperty> propertySetter, string title)
        {

            return new PropertyViewModel<TProperty>(
            () =>
            {
                TProperty getProperty(TEntity entity)
                {
                    var layout = GetLayout(scoreLayoutProvider, entity);
                    var property = propertyGetter(layout);
                    return property;
                }

                var entities = notes;
                TEntity firstEntity = entities.First();
                TProperty firstValue = getProperty(firstEntity);
                return !entities.All(m => getProperty(m)!.Equals(firstValue)) ? default! : firstValue;
            },
            (val) =>
            {
                scoreBuilder
                    .Edit<TEditor>(notes.Select(e => e.Id), (element) =>
                    {
                        var layout = element.ReadLayout();
                        propertySetter(layout, val);
                        element.Apply(layout);
                    })
                    .Build();

            },
            title);
        }
    }
}
