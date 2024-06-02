using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Extensions;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;
using System.Diagnostics.CodeAnalysis;

namespace Sinfonia.Implementations.ScoreDocument;

public class ScoreMeasureEditorProxy(ScoreMeasure source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IScoreMeasureEditor
{
    private readonly ScoreMeasure source = source;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;



    public int IndexInScore => source.IndexInScore;

    public TimeSignature TimeSignature => source.TimeSignature;

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
        return source.UserLayout;
    }

    public void RemoveLayout()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<AuthorScoreMeasureLayout, ScoreMeasureLayoutMembers>(source.AuthorLayout).ThenInvalidate(notifyEntityChanged, source.HostDocument);
        transaction.Enqueue(command);
    }

    public void SetKeySignature(KeySignature keySignature)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorScoreMeasureLayout, ScoreMeasureLayoutMembers>(source.AuthorLayout, s => s.KeySignature = keySignature).ThenInvalidate(notifyEntityChanged, source.HostDocument);
        transaction.Enqueue(command);
    }

    public void SetPaddingLeft(double padding)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorScoreMeasureLayout, ScoreMeasureLayoutMembers>(source.AuthorLayout, s => s.PaddingLeft = padding).ThenInvalidate(notifyEntityChanged, source.HostDocument);
        transaction.Enqueue(command);
    }

    public void SetPaddingRight(double padding)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorScoreMeasureLayout, ScoreMeasureLayoutMembers>(source.AuthorLayout, s => s.PaddingRight = padding).ThenInvalidate(notifyEntityChanged, source.HostDocument);
        transaction.Enqueue(command);
    }

    public void SetPaddingBottom(double? padding)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorScoreMeasureLayout, ScoreMeasureLayoutMembers>(source.AuthorLayout, s => s.PaddingBottom = padding).ThenInvalidate(notifyEntityChanged, source.HostDocument);
        transaction.Enqueue(command);
    }
}
