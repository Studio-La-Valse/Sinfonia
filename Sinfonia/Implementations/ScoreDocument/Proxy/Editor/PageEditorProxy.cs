namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class PageEditorProxy : IPageEditor
{
    private readonly Page page;
    private readonly ICommandManager commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;


    public int IndexInScore => page.IndexInScore;



    public PageEditorProxy(Page page, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
    {
        this.page = page;
        this.commandManager = commandManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }


    public IPageLayout ReadLayout()
    {
        return page.Layout;
    }

    public IEnumerable<IStaffSystemEditor> EnumerateStaffSystems()
    {
        return page.StaffSystems.Where(s => s.ScoreMeasures.Count > 0).Select(s => s.ProxyEditor(commandManager, notifyEntityChanged));
    }
}