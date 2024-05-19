using Sinfonia.Implementations.ScoreDocument.Layout;
using StudioLaValse.ScoreDocument.Models;

namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class NoteEditorProxy(Note source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : INoteEditor, IUniqueScoreElement
{
    private readonly Note source = source;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;


    public InstrumentMeasure HostMeasure => source.HostMeasure;

    public Pitch Pitch => source.Pitch;

    public bool Grace => source.Grace;

    public Position Position => source.Position;

    public RythmicDuration RythmicDuration => source.RythmicDuration;

    public Tuplet Tuplet => source.Tuplet;

    public Guid Guid => source.Guid;

    public int Id => source.Id;


    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        yield break;
    }

    public INoteLayout ReadLayout()
    {
        return source.SecondaryLayout;
    }

    public void RemoveLayout()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<SecondaryNoteLayout, NoteLayoutModel>(source.SecondaryLayout).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public void SetForceAccidental(AccidentalDisplay display)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<SecondaryNoteLayout, NoteLayoutModel>(source.SecondaryLayout, s => s.ForceAccidental = display).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public void SetScale(double scale)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<SecondaryNoteLayout, NoteLayoutModel>(source.SecondaryLayout, s => s.Scale = scale).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public void SetStaffIndex(int staffIndex)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<SecondaryNoteLayout, NoteLayoutModel>(source.SecondaryLayout, s => s.StaffIndex = staffIndex).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public void SetXOffset(double offset)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<SecondaryNoteLayout, NoteLayoutModel>(source.SecondaryLayout, s => s.XOffset = offset).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }
}
