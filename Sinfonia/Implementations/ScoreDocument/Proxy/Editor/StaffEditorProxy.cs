namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class StaffEditorProxy(Staff staff, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IStaffEditor, IUniqueScoreElement
{
    private readonly Staff staff = staff;
    private readonly ScoreLayoutDictionary scoreLayoutDictionary = scoreLayoutDictionary;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;


    public int IndexInStaffGroup => staff.IndexInStaffGroup;

    public Guid Guid => staff.Guid;

    public int Id => staff.Id;

    public ScoreDocumentCore HostScoreDocument => staff.HostScoreDocument;

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        yield break;
    }

    public void Apply(StaffLayout layout)
    {
        scoreLayoutDictionary.Apply(this, layout);
    }

    public StaffLayout ReadLayout()
    {
        return scoreLayoutDictionary.StaffLayout(this);
    }

    public void RemoveLayout()
    {
        scoreLayoutDictionary.Restore(this);
    }
}
