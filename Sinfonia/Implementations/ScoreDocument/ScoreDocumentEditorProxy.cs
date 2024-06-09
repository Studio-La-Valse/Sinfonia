using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using StudioLaValse.ScoreDocument.Reader.Extensions;

namespace Sinfonia.Implementations.ScoreDocument;

public class ScoreDocumentEditorProxy(ScoreDocumentCore score, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreDocumentEditor
{
    private readonly ScoreDocumentCore score = score;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;

    public int NumberOfMeasures => score.NumberOfMeasures;

    public int NumberOfInstruments => score.NumberOfInstruments;

    public int Id => score.Id;

    public void AddInstrumentRibbon(Instrument instrument)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.AddInstrumentRibbon(instrument)).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
    }

    public void RemoveInstrumentRibbon(int indexInScore)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.RemoveInstrumentRibbon(indexInScore)).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
    }

    public void AppendScoreMeasure(TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.AppendScoreMeasure(timeSignature)).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
    }

    public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.InsertScoreMeasure(index, timeSignature)).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
    }

    public void RemoveScoreMeasure(int index)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.RemoveScoreMeasure(index)).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
    }

    public void Clear()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentModel>(score, s => s.Clear()).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
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
        return score.AuthorLayout;
    }

    public void RemoveLayout()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<AuthorScoreDocumentLayout, ScoreDocumentLayoutMembers>(score.AuthorLayout).ThenInvalidate(notifyEntityChanged, score.ProxyReader());
        transaction.Enqueue(command);
    }
}
