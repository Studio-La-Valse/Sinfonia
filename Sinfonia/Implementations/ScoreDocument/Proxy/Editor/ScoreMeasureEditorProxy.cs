using Sinfonia.Extensions;
using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;
using StudioLaValse.ScoreDocument.Core;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class ScoreMeasureEditorProxy(ScoreMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreMeasureEditor, IUniqueScoreElement
{
    private readonly ScoreMeasure source = source;
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

    public bool TryReadNext([NotNullWhen(true)] out IScoreMeasureEditor? next)
    {
        _ = source.TryReadNext(out var _next);
        next = _next?.ProxyEditor(commandManager, notifyEntityChanged);
        return next != null;
    }

    public bool TryReadPrevious([NotNullWhen(true)] out IScoreMeasureEditor? previous)
    {
        _ = source.TryReadPrevious(out var _previous);
        previous = _previous?.ProxyEditor(commandManager, notifyEntityChanged);
        return previous != null;
    }

    public IInstrumentMeasureEditor ReadMeasure(int ribbonIndex)
    {
        return source.GetMeasureCore(ribbonIndex).ProxyEditor(commandManager, notifyEntityChanged);
    }

    public IEnumerable<IInstrumentMeasureEditor> ReadMeasures()
    {
        return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        return ReadMeasures();
    }

    public IScoreMeasureLayout ReadLayout()
    {
        return source.Layout;
    }

    public void RemoveLayout()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<ScoreMeasureLayout, ScoreMeasureLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
        transaction.Enqueue(command);
    }
}
