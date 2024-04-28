namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class StaffGroupEditorProxy(StaffGroup staffGroup, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IStaffGroupEditor
{
    private readonly StaffGroup staffGroup = staffGroup;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;




    public Instrument Instrument => staffGroup.Instrument;

    public int IndexInSystem => staffGroup.IndexInSystem;

    public IInstrumentRibbonEditor InstrumentRibbon => staffGroup.InstrumentRibbon.ProxyEditor(commandManager, notifyEntityChanged);



    public IEnumerable<IInstrumentMeasureEditor> EnumerateMeasures()
    {
        return staffGroup.EnumerateMeasures().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IEnumerable<IStaffEditor> EnumerateStaves(int numberOfStaves)
    {
        return staffGroup.EnumerateStaves(numberOfStaves).Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
    }

    public IStaffGroupLayout ReadLayout()
    {
        return staffGroup.Layout;
    }
}
