namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class StaffGroupEditorProxy(StaffGroup staffGroup, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged) : IStaffGroupEditor, IUniqueScoreElement
{
    private readonly StaffGroup staffGroup = staffGroup;
    private readonly ScoreLayoutDictionary scoreLayoutDictionary = scoreLayoutDictionary;
    private readonly ICommandManager commandManager = commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged = notifyEntityChanged;




    public Instrument Instrument => staffGroup.Instrument;

    public int IndexInSystem => staffGroup.IndexInSystem;

    public IInstrumentRibbonEditor InstrumentRibbon => staffGroup.InstrumentRibbon.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);

    public Guid Guid => staffGroup.Guid;

    public int Id => staffGroup.Id;




    public IEnumerable<IInstrumentMeasureEditor> EnumerateMeasures()
    {
        return staffGroup.EnumerateMeasures().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }

    public IEnumerable<IStaffEditor> EnumerateStaves(int numberOfStaves)
    {
        return staffGroup.EnumerateStaves(numberOfStaves).Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
    }

    public IEnumerable<IScoreElement> EnumerateChildren()
    {
        foreach (IInstrumentMeasureEditor measure in EnumerateMeasures())
        {
            yield return measure;
        }

        foreach (StaffEditorProxy? staff in staffGroup.EnumerateStaves().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged)))
        {
            yield return staff;
        }
    }

    public StaffGroupLayout ReadLayout()
    {
        return scoreLayoutDictionary.StaffGroupLayout(this);
    }

    public void ApplyLayout(StaffGroupLayout layout)
    {
        scoreLayoutDictionary.Apply(this, layout);
    }

    public void RemoveLayout()
    {
        scoreLayoutDictionary.Restore(this);
    }
}
