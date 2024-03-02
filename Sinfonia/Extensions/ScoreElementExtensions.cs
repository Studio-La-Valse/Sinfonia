namespace Sinfonia.Extensions
{
    public static class ScoreElementExtensions
    {
        public static IEnumerable<IScoreElement> EnumerateAllChildren(this IScoreElement editor)
        {
            return editor.SelectRecursive(e => e.EnumerateChildren());
        }
    }
}
