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

    public Guid Guid => throw new NotImplementedException();

    public void ApplyLayout(PageLayout layout)
    {
        scoreLayoutDictionary.Apply(this, layout);
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        return page.StaffSystems.Select(s => s.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }

    public PageLayout ReadLayout()
    {
        return scoreLayoutDictionary.PageLayout(this);
    }

    public void RemoveLayout()
    {
        scoreLayoutDictionary.Restore(this);
    }
}