using StudioLaValse.ScoreDocument;
using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Extensions;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument;

public class NoteEditorProxy(Note source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : INoteEditor
{
    private readonly Note source = source;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;


    public Pitch Pitch => source.Pitch;

    public Position Position => source.Position;

    public RythmicDuration RythmicDuration => source.RythmicDuration;

    public Tuplet Tuplet => source.Tuplet;

    public int Id => source.Id;



    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        yield break;
    }

    public INoteLayout ReadLayout()
    {
        return source.AuthorLayout;
    }

    public void RemoveLayout()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<AuthorNoteLayout, NoteLayoutMembers>(source.AuthorLayout).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public void SetForceAccidental(AccidentalDisplay display)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorNoteLayout, NoteLayoutMembers>(source.AuthorLayout, s => s.ForceAccidental = display).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public void SetScale(double scale)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorNoteLayout, NoteLayoutMembers>(source.AuthorLayout, s => s.Scale = scale).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public void SetStaffIndex(int staffIndex)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorNoteLayout, NoteLayoutMembers>(source.AuthorLayout, s => s.StaffIndex = staffIndex).ThenInvalidate(notifyEntityChanged, source.HostMeasure.HostMeasure.HostDocument);
        transaction.Enqueue(command);
    }

    public void SetXOffset(double offset)
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new MementoCommand<AuthorNoteLayout, NoteLayoutMembers>(source.AuthorLayout, s => s.XOffset = offset).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
        transaction.Enqueue(command);
    }

    public bool Equals(IUniqueScoreElement? other)
    {
        if (other is null)
        {
            return false;
        }

        return other.Id == Id;
    }
}
