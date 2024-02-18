namespace Sinfonia.Extensions
{
    public static class ScoreElementExtensions
    {
        public static IEnumerable<IUniqueScoreElement> EnumerateAllChildren(this IUniqueScoreElement editor)
        {
            return editor.SelectRecursive(e => e.EnumerateChildren());
        }

        public static IScoreBuilder EditElements<TElement>(this IScoreBuilder builder, IEnumerable<int> elementIds, Action<TElement> action) where TElement : IUniqueScoreElement
        {
            Action<IScoreDocumentEditor> _action = (scoreBuilder) =>
            {
                foreach (var child in scoreBuilder.EnumerateAllChildren().OfType<TElement>())
                {
                    if (elementIds.Contains(child.Id))
                    {
                        action(child);
                    }
                }
            };

            return builder.Edit(_action);
        }
    }
}
