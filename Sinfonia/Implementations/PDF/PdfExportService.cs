using Avalonia.Platform.Storage;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharp.Snippets.Font;
using Sinfonia.ViewModels.Application;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Drawable;
using StudioLaValse.ScoreDocument.Drawable.Scenes;
using StudioLaValse.ScoreDocument.Reader;
using StudioLaValse.ScoreDocument.Reader.Extensions;
using ColorARGB = StudioLaValse.Geometry.ColorARGB;

namespace Sinfonia.Implementations.PDF;
internal class PdfExportService : IPdfExportService
{
    static PdfExportService()
    {
        var systemFontResolver = new SystemFontResolver();
        var resourceFontResolver = new ResourceFontResolver();
        var fontResolver = new FontResolver(systemFontResolver, resourceFontResolver);
        GlobalFontSettings.FontResolver = fontResolver;
    }

    private readonly DocumentCollectionViewModel documentCollectionViewModel;
    private readonly MainWindow mainWindow;
    private readonly IUnitToPixelConverter unitToPixelConverter;

    public PdfExportService(DocumentCollectionViewModel documentCollectionViewModel, MainWindow mainWindow, IUnitToPixelConverter unitToPixelConverter)
    {
        this.documentCollectionViewModel = documentCollectionViewModel;
        this.mainWindow = mainWindow;
        this.unitToPixelConverter = unitToPixelConverter;
    }

    public void Export()
    {
        var activeDocument = documentCollectionViewModel.TryGetActiveDocument(out var d) ? d : throw new Exception();
        var scoreDocumentReader = activeDocument.ScoreDocumentReader;
        var visualPageFactory = activeDocument.CanvasViewModel.VisualPageFactory;

        var task = mainWindow.StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions()
        {
            Title = "Save PDF...",
            DefaultExtension = ".pdf",
            SuggestedFileName = "my document.pdf",
            ShowOverwritePrompt = true,
            FileTypeChoices = [new FilePickerFileType("PDF files") { Patterns = ["*.pdf"] }]
        });
        var result = AsyncHelper.RunSync(() => task);
        if (result is null)
        {
            //operation aborted.
            return;
        }

        var bytes = GenerateBytes(scoreDocumentReader, visualPageFactory);

        using (var stream = AsyncHelper.RunSync(result.OpenWriteAsync))
        {
            bytes.Save(stream);
        };
    }

    public PdfDocument GenerateBytes(IScoreDocumentReader scoreDocumentReader, IVisualPageFactory visualPageFactory)
    {
        var pdfDocument = new PdfDocument();

        foreach (var page in scoreDocumentReader.ReadPages(12d / 72d * 25.4 / 4))
        {
            var pageLayout = page.ReadLayout();
            var pageWidth = pageLayout.PageWidth;
            var pageHeight = pageLayout.PageHeight;
            var pageColor = pageLayout.PageColor;

            var visualPage = visualPageFactory.CreateContent(page, 0, 0);
            var pdfPage = pdfDocument.AddPage();
            pdfPage.Width = unitToPixelConverter.UnitsToPixels(pageWidth);
            pdfPage.Height = unitToPixelConverter.UnitsToPixels(pageHeight);

            using var graphics = XGraphics.FromPdfPage(pdfPage);
            var painter = new PdfPageCanvasPainter(graphics, pageWidth, pageHeight);
            var _pageColor = new ColorARGB(pageColor.A, pageColor.R, pageColor.G, pageColor.B);
            painter.DrawBackground(_pageColor);
            painter.Draw(visualPage);
            painter.FinishDrawing();
        }

        return pdfDocument;
    }
}
