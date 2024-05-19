using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Layout.Templates;
using StudioLaValse.ScoreDocument.Models;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class ScoreDocumentEditorProxy(ScoreDocumentCore score, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreDocumentEditor, IUniqueScoreElement
{
    private readonly ScoreDocumentCore score = score;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;

    public int NumberOfMeasures => score.NumberOfMeasures;

    public int NumberOfInstruments => score.NumberOfInstruments;

    public Guid Guid => score.Guid;

    public int Id => score.Id;



    public void AddInstrumentRibbon(Instrument instrument)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentModel> command = new(score, s => s.AddInstrumentRibbon(instrument));
        transaction.Enqueue(command);
    }

    public void RemoveInstrumentRibbon(int indexInScore)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentModel> command = new(score, s => s.RemoveInstrumentRibbon(indexInScore));
        transaction.Enqueue(command);
    }

    public void AppendScoreMeasure(TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentModel> command = new(score, s => s.AppendScoreMeasure(timeSignature));
        transaction.Enqueue(command);
    }

    public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentModel> command = new(score, s => s.InsertScoreMeasure(index, timeSignature));
        transaction.Enqueue(command);
    }

    public void RemoveScoreMeasure(int index)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentModel> command = new(score, s => s.RemoveScoreMeasure(index));
        transaction.Enqueue(command);
    }

    public void Clear()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentModel> command = new(score, s => s.Clear());
        transaction.Enqueue(command);
    }




    public IEnumerable<IPageEditor> GeneratePages()
    {
        return score.GeneratePages().Select(p => p.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IEnumerable<IScoreMeasureEditor> ReadScoreMeasures()
    {
        return score.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IEnumerable<IInstrumentRibbonEditor> ReadInstrumentRibbons()
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
    }



    public IInstrumentRibbonEditor ReadInstrumentRibbon(int indexInScore)
    {
        return score.GetInstrumentRibbonCore(indexInScore).ProxyEditor(commandManager, notifyEntityChanged);
    }

    public IScoreMeasureEditor ReadScoreMeasure(int indexInScore)
    {
        return score.GetScoreMeasureCore(indexInScore).ProxyEditor(commandManager, notifyEntityChanged);
    }



    public IScoreDocumentLayout ReadLayout()
    {
        return score.SecondaryLayout;
    }

    public void RemoveLayout()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<SecondaryScoreDocumentLayout, ScoreDocumentLayoutModel>(score.SecondaryLayout).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
    }

}
