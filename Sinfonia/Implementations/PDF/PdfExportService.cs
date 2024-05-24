using Avalonia.Controls.Documents;
using Avalonia.Platform.Storage;
using PdfSharp.Drawing;
using PdfSharp.Fonts;
using PdfSharp.Pdf;
using PdfSharpTest;
using Sinfonia.Implementations.CanvasPainters;
using Sinfonia.ViewModels.Application;
using Sinfonia.Windows;
using StudioLaValse.ScoreDocument.Drawable.Scenes;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Reader;
using StudioLaValse.ScoreDocument.Reader.Extensions;
using System.Diagnostics;
using static PdfSharp.Snippets.Font.SegoeWpFontResolver;
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

    public PdfExportService(DocumentCollectionViewModel documentCollectionViewModel, MainWindow mainWindow)
    {
        this.documentCollectionViewModel = documentCollectionViewModel;
        this.mainWindow = mainWindow;
    }

    public void Export()
    {
        var activeDocument = documentCollectionViewModel.TryGetActiveDocument(out var d) ? d : throw new Exception();
        var scoreDocumentReader = activeDocument.ScoreDocumentReader;
        var scoreStyleTemplate = activeDocument.CanvasViewModel.ScoreDocumentStyle;
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

        var bytes = GenerateBytes(scoreDocumentReader, visualPageFactory, scoreStyleTemplate);

        using (var stream = AsyncHelper.RunSync(result.OpenWriteAsync))
        {
            bytes.Save(stream);
        };
    }

    public PdfDocument GenerateBytes(IScoreDocumentReader scoreDocumentReader, IVisualPageFactory visualPageFactory, ScoreDocumentStyleTemplate scoreDocumentStyleTemplate)
    {
        var pdfDocument = new PdfDocument();
        var pageColor = scoreDocumentStyleTemplate.PageColor;
        var pageWidth = scoreDocumentStyleTemplate.PageStyleTemplate.PageWidth;
        var pageHeight = scoreDocumentStyleTemplate.PageStyleTemplate.PageHeight;

        foreach (var page in scoreDocumentReader.ReadPages(scoreDocumentStyleTemplate))
        {
            var visualPage = visualPageFactory.CreateContent(page);
            var pdfPage = pdfDocument.AddPage();
            pdfPage.Width = pageWidth;
            pdfPage.Height = pageHeight;

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
