using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Reader.Extensions;

namespace Sinfonia.Implementations.ScoreDocument
{
    public class PageGenerator
    {
        private readonly ScoreDocumentStyleTemplate styleTemplate;


        public PageGenerator(ScoreDocumentStyleTemplate styleTemplate)
        {
            this.styleTemplate = styleTemplate;
        }

        public IEnumerable<Page> Generate(ScoreDocumentCore scoreDocument)
        {
            var scoreDocumentLayout = scoreDocument.Layout;

            var currentpage = new Page(0, scoreDocument, styleTemplate);
            currentpage.StaffSystems.Clear();
            var currentSystem = new StaffSystem(scoreDocument, styleTemplate);
            currentpage.StaffSystems.Add(currentSystem);

            var pageLayout = currentpage.Layout;
            var pageWidth = pageLayout.PageWidth;
            var pageHeight = pageLayout.PageHeight;
            var pageMarginBottom = pageLayout.MarginBottom;

            var systemIndex = 1;
            var pageIndex = 1;
            var currentSystemCanvasTop = pageLayout.MarginTop;
            var lineSpacing = 1.2;

            foreach (var measure in scoreDocument.EnumerateMeasuresCore())
            {
                currentSystem.ScoreMeasures.Add(measure);

                var currentSystemLength = currentSystem.ScoreMeasures.Select(m => m.Layout.Width).Sum();
                var currentAvailableWidth = pageWidth - pageLayout.MarginLeft - pageLayout.MarginRight;
                // Need to add a new system.
                if (currentSystemLength > currentAvailableWidth)
                {
                    var previousSystemHeight = currentSystem.Proxy().CalculateHeight(lineSpacing, scoreDocumentLayout);
                    var previousSystemMarginBottom = currentSystem.Layout.PaddingBottom;
                    currentSystem = new StaffSystem(scoreDocument, styleTemplate);
                    currentSystemCanvasTop += previousSystemHeight + previousSystemMarginBottom;

                    var currentSystemCanvasBottom = currentSystemCanvasTop + currentSystem.Proxy().CalculateHeight(lineSpacing, scoreDocumentLayout);
                    var currentLowestAllowedPoint = pageHeight - pageMarginBottom;
                    // Need to add a new page.
                    if (currentSystemCanvasBottom > currentLowestAllowedPoint)
                    {
                        yield return currentpage;
                        currentpage = new Page(pageIndex, scoreDocument, styleTemplate);
                        currentpage.StaffSystems.Clear();
                        pageLayout = currentpage.Layout;
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
