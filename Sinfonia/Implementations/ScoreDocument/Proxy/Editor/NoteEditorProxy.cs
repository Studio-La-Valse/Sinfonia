using Sinfonia.Implementations.Commands;
using Sinfonia.Implementations.ScoreDocument.Layout;
using Sinfonia.Implementations.ScoreDocument.Memento.Layout;
using Sinfonia.Implementations.ScoreDocument.Proxy.Reader;

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
        return source.Layout;
    }

    public void RemoveLayout()
    {
        var transaction = commandManager.ThrowIfNoTransactionOpen();
        var command = new RestoreLayoutCommand<NoteLayout, NoteLayoutMemento>(source.Layout).ThenInvalidate(notifyEntityChanged, source.ProxyReader());
        transaction.Enqueue(command);
    }

    public void SetStaffIndex(int staffIndex)
    {
        source.Layout.StaffIndex = staffIndex;
    }
}
