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

    public KeySignature KeySignature => source.Layout.KeySignature;

    public Guid Guid => source.Guid;

    public int Id => source.Id;



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
        var command = new RestoreLayoutCommand<ScoreMeasureLayout, ScoreMeasureLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.ScoreDocumentCore);
        transaction.Enqueue(command);
    }

    public void SetKeySignature(KeySignature keySignature)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.Layout.KeySignature = keySignature).ThenInvalidate(notifyEntityChanged, source.ScoreDocumentCore);
        transaction.Enqueue(command);
    }

    public void SetPaddingLeft(double padding)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.Layout.PaddingLeft = padding).ThenInvalidate(notifyEntityChanged, source.ScoreDocumentCore);
        transaction.Enqueue(command);
    }

    public void SetPaddingRight(double padding)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.Layout.PaddingRight = padding).ThenInvalidate(notifyEntityChanged, source.ScoreDocumentCore);
        transaction.Enqueue(command);
    }

    public void SetWidth(double width)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.Layout.Width = width).ThenInvalidate(notifyEntityChanged, source.ScoreDocumentCore);
        transaction.Enqueue(command);
    }

    public void SetPaddingBottom(double? padding)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<ScoreMeasure, ScoreMeasureMemento>(source, s => s.Layout.PaddingBottom = padding).ThenInvalidate(notifyEntityChanged, source.ScoreDocumentCore);
        transaction.Enqueue(command);
    }
}
