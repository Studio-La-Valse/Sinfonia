using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Layout.Extensions;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class PageGenerator
    {
        private readonly Dictionary<int, Page> pages = [];
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly IScoreDocumentLayout scoreLayoutProvider;
        private readonly IList<(Guid guid, int id, Dictionary<Guid, (Guid guid, int id, IList<(Guid guid, int id)> staves)> staffGroups)> staffSystems = [];

        public PageGenerator(IKeyGenerator<int> keyGenerator, IScoreDocumentLayout scoreLayoutProvider)
        {
            this.keyGenerator = keyGenerator;
            this.scoreLayoutProvider = scoreLayoutProvider;
        }

        private StaffSystem GetAppendOrThrow(int index, ScoreDocumentCore scoreDocument)
        {
            ArgumentOutOfRangeException.ThrowIfNegative(index, nameof(index));

            if (index < staffSystems.Count)
            {
                var (guid, id, staffGroups) = staffSystems[index];
                return new StaffSystem(scoreDocument, keyGenerator, guid, id, staffGroups);
            }

            if (index == staffSystems.Count)
            {
                var (guid, id, staffGroups) = (Guid.NewGuid(), keyGenerator.Generate(), new Dictionary<Guid, (Guid guid, int id, IList<(Guid, int)>)>());
                staffSystems.Add((guid, id, staffGroups));
                return new StaffSystem(scoreDocument, keyGenerator, guid, id, staffGroups);
            }

            throw new ArgumentOutOfRangeException(nameof(index));
        }

        private Page GetOrCreate(int index)
        {
            if (pages.TryGetValue(index, out var page))
            {
                return page;
            }

            pages[index] = new Page(index, keyGenerator.Generate(), Guid.NewGuid());
            return GetOrCreate(index);
        }

        public IEnumerable<Page> Generate(ScoreDocumentCore scoreDocument)
        {
            var currentpage = GetOrCreate(0);
            currentpage.StaffSystems.Clear();
            var currentSystem = GetAppendOrThrow(0, scoreDocument);
            currentpage.StaffSystems.Add(currentSystem);

            var pageLayout = scoreLayoutProvider.PageLayout(currentpage.Proxy());
            var pageWidth = pageLayout.PageWidth;
            var pageHeight = pageLayout.PageHeight;
            var pageMarginBottom = pageLayout.MarginBottom;

            var systemIndex = 1;
            var pageIndex = 1;
            var currentSystemCanvasTop = pageLayout.MarginTop;
            foreach (var measure in scoreDocument.EnumerateMeasuresCore())
            {
                currentSystem.ScoreMeasures.Add(measure);

                var currentSystemLength = currentSystem.ScoreMeasures.Select(m => scoreLayoutProvider.ScoreMeasureLayout(m.Proxy()).Width).Sum();
                var currentAvailableWidth = pageWidth - pageLayout.MarginLeft - pageLayout.MarginRight;
                // Need to add a new system.
                if (currentSystemLength > currentAvailableWidth)
                {
                    var previousSystemHeight = currentSystem.Proxy().CalculateHeight(scoreLayoutProvider);
                    var previousSystemMarginBottom = scoreLayoutProvider.StaffSystemLayout(currentSystem.Proxy()).PaddingBottom;
                    currentSystem = GetAppendOrThrow(systemIndex, scoreDocument);
                    currentSystemCanvasTop += previousSystemHeight + previousSystemMarginBottom;

                    var currentSystemCanvasBottom = currentSystemCanvasTop + currentSystem.Proxy().CalculateHeight(scoreLayoutProvider);
                    var currentLowestAllowedPoint = pageHeight - pageMarginBottom;
                    // Need to add a new page.
                    if (currentSystemCanvasBottom > currentLowestAllowedPoint)
                    {
                        yield return currentpage;
                        currentpage = GetOrCreate(pageIndex);
                        currentpage.StaffSystems.Clear();
                        pageLayout = scoreLayoutProvider.PageLayout(currentpage.Proxy());
                        pageWidth = pageLayout.PageWidth;
                        pageHeight = pageLayout.PageHeight;
                        pageMarginBottom = pageLayout.MarginBottom;

                        currentSystemCanvasTop = pageLayout.MarginTop;
                        pageIndex++;
                    }

                    currentpage.StaffSystems.Add(currentSystem);
                    systemIndex++;
                }
            }
            if (currentpage.StaffSystems.Any(s => s.ScoreMeasures.Count > 0))
            {
                yield return currentpage;
            }
        }
    }
}
