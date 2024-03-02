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
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(score, s => s.AddInstrumentRibbon(instrument));
        transaction.Enqueue(command);
    }

    public void RemoveInstrumentRibbon(int indexInScore)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(score, s => s.RemoveInstrumentRibbon(indexInScore));
        transaction.Enqueue(command);
    }


    public void AppendScoreMeasure(TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(score, s => s.AppendScoreMeasure(timeSignature));
        transaction.Enqueue(command);
    }

    public void InsertScoreMeasure(int index, TimeSignature? timeSignature = null)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(score, s => s.InsertScoreMeasure(index, timeSignature));
        transaction.Enqueue(command);
    }

    public void RemoveScoreMeasure(int index)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(score, s => s.RemoveScoreMeasure(index));
        transaction.Enqueue(command);
    }

    public void Clear()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreDocumentCore, ScoreDocumentMemento>(score, s => s.Clear());
        transaction.Enqueue(command);
    }

    public IEnumerable<IStaffSystemEditor> EnumerateStaffSystems()
    {
        return score.EnumerateStaffSystems().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
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

    public void ApplyLayout(ScoreDocumentLayout layout)
    {
        scoreLayoutDictionary.Apply(layout);
    }

    public ScoreDocumentLayout ReadLayout()
    {
        return scoreLayoutDictionary.DocumentLayout(this);
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

        foreach (var system in EnumerateStaffSystems())
        {
            yield return system;
        }
    }
}
