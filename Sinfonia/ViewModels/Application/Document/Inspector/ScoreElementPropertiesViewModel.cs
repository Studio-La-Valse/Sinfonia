using Sinfonia.ViewModels.Base;


namespace Sinfonia.ViewModels.Application.Document.Inspector
{
    public abstract class ScoreElementPropertiesViewModel<TEntity, TEditor> : PropertyCollectionViewModel
            where TEntity : IUniqueScoreElement
            where TEditor : IScoreElementEditor
    {
        private readonly IScoreBuilder scoreBuilder;
        private readonly IEnumerable<TEntity> notes;

        internal ScoreElementPropertiesViewModel(IScoreBuilder scoreBuilder, IEnumerable<TEntity> notes)
        {
            this.scoreBuilder = scoreBuilder;
            this.notes = notes;
        }

        protected PropertyViewModel<TProperty> Create<TProperty>(Func<TEntity, TProperty> propertyGetter, Action<TEditor, TProperty> propertySetter, string title)
        {

            return new PropertyViewModel<TProperty>(
            () =>
            {
                TProperty getProperty(TEntity entity)
                {
                    return propertyGetter(entity);
                }

                var entities = notes;
                var firstEntity = entities.First();
                var firstValue = getProperty(firstEntity);
                return !entities.All(m => getProperty(m)!.Equals(firstValue)) ? default! : firstValue;
            },
            (val) =>
            {
                scoreBuilder
                    .Edit<TEditor>(notes.Select(e => e.Id), (element) =>
                    {
                        propertySetter(element, val);
                    })
                    .Build();

            },
            title);
        }
    }
}
