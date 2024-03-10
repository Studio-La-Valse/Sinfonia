using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Drawable.Extensions;

namespace Sinfonia.Implementations.ScoreDocument
{
    internal class PageGenerator
    {
        private readonly Dictionary<int, Page> pages = [];
        private readonly IKeyGenerator<int> keyGenerator;
        private readonly IScoreLayoutProvider scoreLayoutProvider;
        private readonly IList<(Guid guid, int id, Dictionary<Guid, (Guid guid, int id, IList<(Guid guid, int id)> staves)> staffGroups)> staffSystems = [];

        public PageGenerator(IKeyGenerator<int> keyGenerator, IScoreLayoutProvider scoreLayoutProvider)
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

        public Page GetOrCreate(int index)
        {
            if(pages.TryGetValue(index, out var page))
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
            var pageWidth = pageLayout.PageWidth.Value;
            var pageHeight = pageLayout.PageHeight.Value;
            var pageMarginBottom = pageLayout.MarginBottom.Value;

            var systemIndex = 1;
            var pageIndex = 1;
            var currentSystemCanvasTop = pageLayout.MarginTop.Value;
            foreach (ScoreMeasure measure in scoreDocument.EnumerateMeasuresCore())
            {
                currentSystem.ScoreMeasures.Add(measure);

                var currentSystemLength = currentSystem.ScoreMeasures.Select(m => scoreLayoutProvider.ScoreMeasureLayout(m.Proxy()).Width.Value).Sum();
                var currentAvailableWidth = pageWidth - pageLayout.MarginLeft.Value - pageLayout.MarginRight.Value;
                // Need to add a new system.
                if(currentSystemLength > currentAvailableWidth)
                {
                    var previousSystemHeight = currentSystem.Proxy().CalculateHeight(scoreLayoutProvider);
                    var previousSystemMarginBottom = scoreLayoutProvider.StaffSystemLayout(currentSystem.Proxy()).PaddingBottom.Value;
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
                        pageWidth = pageLayout.PageWidth.Value;
                        pageHeight = pageLayout.PageHeight.Value;
                        pageMarginBottom = pageLayout.MarginBottom.Value;

                        currentSystemCanvasTop = pageLayout.MarginTop.Value;
                        pageIndex++;
                    }

                    currentpage.StaffSystems.Add(currentSystem);
                    systemIndex++;
                }
            }
            if(currentpage.StaffSystems.Any(s => s.ScoreMeasures.Count > 0))
            {
                yield return currentpage;
            }
        }
    }
}
