namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class NoteEditorProxy(Note source, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : INoteEditor, IUniqueScoreElement
{
    private readonly Note source = source;
    private readonly ScoreLayoutDictionary scoreLayoutDictionary = scoreLayoutDictionary;
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

    public void Apply(NoteLayout layout)
    {
        scoreLayoutDictionary.Apply(this, layout);
    }

    public NoteLayout ReadLayout()
    {
        return scoreLayoutDictionary.NoteLayout(this);
    }

    public void RemoveLayout()
    {
        scoreLayoutDictionary.Restore(this);
    }
}
