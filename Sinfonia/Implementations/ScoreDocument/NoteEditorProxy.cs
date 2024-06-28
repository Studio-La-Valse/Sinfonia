using StudioLaValse.ScoreDocument.Implementation;
using StudioLaValse.ScoreDocument.Implementation.Layout;
using StudioLaValse.ScoreDocument.Models;
using StudioLaValse.ScoreDocument.Models.Base;

namespace Sinfonia.Implementations.ScoreDocument;

public class NoteEditorProxy(Note source, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : INote
{
    private readonly Note source = source;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;


    public Pitch Pitch
    {
        get => source.Pitch;
        set
        {
            var transaction = commandManager.ThrowIfNoTransactionOpen();
            var command = new MementoCommand<Note, NoteModel>(source, s => s.Pitch = value).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
            transaction.Enqueue(command);
        }
    }

    public AuthorNoteLayout Layout => source.AuthorLayout;
    public Position Position => source.Position;
    public RythmicDuration RythmicDuration => source.RythmicDuration;
    public Tuplet Tuplet => source.Tuplet;
    public int Id => source.Id;


    public TemplateProperty<AccidentalDisplay> ForceAccidental => Layout.ForceAccidental.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
    public TemplateProperty<double> Scale => Layout.Scale.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure);
    public TemplateProperty<int> StaffIndex => Layout.StaffIndex.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure.HostMeasure.HostDocument);
    public TemplateProperty<double> XOffset => Layout.XOffset.UseCommandManager(commandManager).ThenInvalidate(notifyEntityChanged, source.HostMeasure);

  
    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        yield break;
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
