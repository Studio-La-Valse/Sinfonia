using Sinfonia.Implementations.Commands;
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
        ITransaction transaction = commandManager.ThrowIfNoTransactionOpen();
        BaseCommand command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.EditKeySignature(keySignature)).ThenInvalidate(notifyEntityChanged, this);
        transaction.Enqueue(command);
    }

    public bool TryReadNext([NotNullWhen(true)] out IScoreMeasureEditor? next)
    {
        _ = source.TryReadNext(out ScoreMeasure? _next);
        next = _next?.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
        return next != null;
    }

    public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasureEditor? previous)
    {
        _ = source.TryReadPrevious(out ScoreMeasure? _previous);
        previous = _previous?.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
        return previous != null;
    }

    public IInstrumentMeasureEditor ReadMeasure(int ribbonIndex)
    {
        return source.GetMeasureCore(ribbonIndex).ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
    }

    public IEnumerable<IInstrumentMeasureEditor> ReadMeasures()
    {
        return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        return ReadMeasures();
    }

    public ScoreMeasureLayout ReadLayout()
    {
        return scoreLayoutDictionary.ScoreMeasureLayout(this);
    }

    public void Apply(ScoreMeasureLayout layout)
    {
        scoreLayoutDictionary.Apply(this, layout);
    }

    public void RemoveLayout()
    {
        scoreLayoutDictionary.Restore(this);
    }
}
