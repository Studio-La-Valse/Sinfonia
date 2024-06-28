using StudioLaValse.ScoreDocument.Extensions;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
namespace Sinfonia.Implementations.ScoreDocument;

public class ScoreDocumentEditorProxy(ScoreDocumentCore score, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreDocument
{
    private readonly ScoreDocumentCore score = score;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;

    public int NumberOfMeasures => score.NumberOfMeasures;

    public int NumberOfInstruments => score.NumberOfInstruments;

    public int Id => score.Id;

    public AuthorScoreDocumentLayout Layout => score.AuthorLayout;

    public ReadonlyTemplateProperty<string> GlyphFamily => Layout.GlyphFamily;

    public ReadonlyTemplateProperty<double> FirstSystemIndent => Layout.FirstSystemIndent;

    public ReadonlyTemplateProperty<double> HorizontalStaffLineThickness => Layout.HorizontalStaffLineThickness;

    public ReadonlyTemplateProperty<double> Scale => Layout.Scale;

    public ReadonlyTemplateProperty<double> StemLineThickness => Layout.StemLineThickness;

    public ReadonlyTemplateProperty<double> VerticalStaffLineThickness => Layout.VerticalStaffLineThickness;

    public ReadonlyTemplateProperty<StudioLaValse.ScoreDocument.Templates.ColorARGB> PageColor => Layout.PageColor;

    public ReadonlyTemplateProperty<StudioLaValse.ScoreDocument.Templates.ColorARGB> PageForegroundColor => Layout.PageForegroundColor;

    public ReadonlyTemplateProperty<double> PageMarginBottom => Layout.PageMarginBottom;

    public ReadonlyTemplateProperty<double> PageMarginLeft => Layout.PageMarginLeft;

    public ReadonlyTemplateProperty<double> PageMarginRight => Layout.PageMarginRight;

    public ReadonlyTemplateProperty<double> PageMarginTop => Layout.PageMarginTop;

    public ReadonlyTemplateProperty<int> PageHeight => Layout.PageHeight;

    public ReadonlyTemplateProperty<int> PageWidth => Layout.PageWidth;

    public ReadonlyTemplateProperty<double> StaffSystemPaddingBottom => Layout.StaffSystemPaddingBottom;

    public ReadonlyTemplateProperty<double> StaffGroupPaddingBottom => Layout.StaffGroupPaddingBottom;

    public ReadonlyTemplateProperty<double> StaffPaddingBottom => Layout.StaffPaddingBottom;

    public void AddInstrumentRibbon(Instrument instrument)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.AddInstrumentRibbon(instrument)).ThenInvalidate(notifyEntityChanged, score);
        transaction.Enqueue(command);
    }

    public void RemoveInstrumentRibbon(int indexInScore)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.RemoveInstrumentRibbon(indexInScore)).ThenInvalidate(notifyEntityChanged, score);
        transaction.Enqueue(command);
    }

    public void AppendScoreMeasure(TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.AppendScoreMeasure(timeSignature)).ThenInvalidate(notifyEntityChanged, score);
        transaction.Enqueue(command);
    }

    public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.InsertScoreMeasure(index, timeSignature)).ThenInvalidate(notifyEntityChanged, score);
        transaction.Enqueue(command);
    }

    public void RemoveScoreMeasure(int index)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.RemoveScoreMeasure(index)).ThenInvalidate(notifyEntityChanged, score);
        transaction.Enqueue(command);
    }

    public void Clear()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.Clear()).ThenInvalidate(notifyEntityChanged, score);
        transaction.Enqueue(command);
    }

    public IEnumerable<IScoreMeasure> ReadScoreMeasures()
    {
        return score.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IEnumerable<IInstrumentRibbon> ReadInstrumentRibbons()
    {
        return score.EnumerateRibbonsCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        foreach (var ribbon in ReadInstrumentRibbons())
        {
            yield return ribbon;
        }

        foreach (var measure in ReadScoreMeasures())
        {
            yield return measure;
        }

        foreach(var page in this.ReadPages())
        {
            yield return page;
        }
    }

    public IInstrumentRibbon ReadInstrumentRibbon(int indexInScore)
    {
        return score.GetInstrumentRibbonCore(indexInScore).ProxyEditor(commandManager, notifyEntityChanged);
    }

    public IScoreMeasure ReadScoreMeasure(int indexInScore)
    {
        return score.GetScoreMeasureCore(indexInScore).ProxyEditor(commandManager, notifyEntityChanged);
    }

    public bool Equals(IUniqueScoreElement? other)
    {
        return other is not null && other.Id == Id;
    }
}
