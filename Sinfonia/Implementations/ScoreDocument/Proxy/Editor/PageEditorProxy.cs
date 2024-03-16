namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class PageEditorProxy : IPageEditor, IUniqueScoreElement
{
    private readonly Page page;
    private readonly ScoreLayoutDictionary scoreLayoutDictionary;
    private readonly ICommandManager commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

    public int Id => page.Id;

    public PageEditorProxy(Page page, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
    {
        this.page = page;
        this.scoreLayoutDictionary = scoreLayoutDictionary;
        this.commandManager = commandManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }

    public Guid Guid => page.Guid;

    public int IndexInScore => page.IndexInScore;

    public void Apply(PageLayout layout)
    {
        scoreLayoutDictionary.Apply(this, layout);
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        return EnumerateStaffSystems();
    }

    public PageLayout ReadLayout()
    {
        return scoreLayoutDictionary.PageLayout(this);
    }

    public void RemoveLayout()
    {
        scoreLayoutDictionary.Restore(this);
    }

    public IEnumerable<IStaffSystemEditor> EnumerateStaffSystems()
    {
        return page.StaffSystems.Where(s => s.ScoreMeasures.Count > 0).Select(s => s.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }
}