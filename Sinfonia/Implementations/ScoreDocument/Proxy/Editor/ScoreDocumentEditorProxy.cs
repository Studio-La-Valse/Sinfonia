
using Sinfonia.Implementations.Commands;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class ScoreDocumentEditorProxy(ScoreDocumentCore score, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreDocumentEditor, IUniqueScoreElement
{
    private readonly ScoreDocumentCore score = score;
    private readonly ScoreLayoutDictionary scoreLayoutDictionary = scoreLayoutDictionary;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;

    public int NumberOfMeasures => score.NumberOfMeasures;

    public int NumberOfInstruments => score.NumberOfInstruments;

    public Guid Guid => score.Guid;

    public int Id => score.Id;


    public void AddInstrumentRibbon(Instrument instrument)
    {
        ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentMemento> command = new(score, s => s.AddInstrumentRibbon(instrument));
        transaction.Enqueue(command);
    }

    public void RemoveInstrumentRibbon(int indexInScore)
    {
        ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentMemento> command = new(score, s => s.RemoveInstrumentRibbon(indexInScore));
        transaction.Enqueue(command);
    }


    public void AppendScoreMeasure(TimeSignature? timeSignature = null)
    {
        ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentMemento> command = new(score, s => s.AppendScoreMeasure(timeSignature));
        transaction.Enqueue(command);
    }

    public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
    {
        ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentMemento> command = new(score, s => s.InsertScoreMeasure(index, timeSignature));
        transaction.Enqueue(command);
    }

    public void RemoveScoreMeasure(int index)
    {
        ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentMemento> command = new(score, s => s.RemoveScoreMeasure(index));
        transaction.Enqueue(command);
    }

    public void Clear()
    {
        ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
        MementoCommand<ScoreDocumentCore, ScoreDocumentMemento> command = new(score, s => s.Clear());
        transaction.Enqueue(command);
    }

    public IEnumerable<IScoreMeasureEditor> ReadScoreMeasures()
    {
        return score.EnumerateMeasuresCore().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }

    public IEnumerable<IInstrumentRibbonEditor> ReadInstrumentRibbons()
    {
        return score.EnumerateRibbonsCore().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }

    public IInstrumentRibbonEditor ReadInstrumentRibbon(int indexInScore)
    {
        return score.GetInstrumentRibbonCore(indexInScore).ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
    }

    public IScoreMeasureEditor ReadScoreMeasure(int indexInScore)
    {
        return score.GetScoreMeasureCore(indexInScore).ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        foreach (IInstrumentRibbonEditor ribbon in ReadInstrumentRibbons())
        {
            yield return ribbon;
        }

        foreach (IScoreMeasureEditor measure in ReadScoreMeasures())
        {
            yield return measure;
        }

        foreach (IPageEditor system in EnumeratePages())
        {
            yield return system;
        }
    }

    public ScoreDocumentLayout ReadLayout()
    {
        return scoreLayoutDictionary.DocumentLayout(this);
    }

    public void Apply(ScoreDocumentLayout layout)
    {
        scoreLayoutDictionary.Apply(this, layout);
    }

    public void RemoveLayout()
    {
        scoreLayoutDictionary.Restore(this);
    }

    public IEnumerable<IPageEditor> EnumeratePages()
    {
        return score.GeneratePages().Select(p => p.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }
}
