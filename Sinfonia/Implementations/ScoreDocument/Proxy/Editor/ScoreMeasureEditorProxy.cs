using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class ScoreMeasureEditorProxy(ScoreMeasure source, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreMeasureEditor, IUniqueScoreElement
{
    private readonly ScoreMeasure source = source;
    private readonly ScoreLayoutDictionary scoreLayoutDictionary = scoreLayoutDictionary;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;



    public int IndexInScore => source.IndexInScore;

    public TimeSignature TimeSignature => source.TimeSignature;

    public KeySignature KeySignature => source.KeySignature;

    public Guid Guid => source.Guid;

    public int Id => source.Id;




    public void EditKeySignature(KeySignature keySignature)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.EditKeySignature(keySignature)).ThenInvalidate(notifyEntityChanged, this);
        transaction.Enqueue(command);
    }

    public IInstrumentMeasureEditor ReadMeasure(int ribbonIndex)
    {
        return source.GetMeasureCore(ribbonIndex).ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
    }

    public IEnumerable<IInstrumentMeasureEditor> ReadMeasures()
    {
        return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }

    public bool TryReadNext([NotNullWhen(true)] out IScoreMeasureEditor? next)
    {
        source.TryReadNext(out var _next);
        next = _next?.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
        return next != null;
    }

    public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasureEditor? previous)
    {
        source.TryReadPrevious(out var _previous);
        previous = _previous?.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
        return previous != null;
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        return ReadMeasures();
    }
}
