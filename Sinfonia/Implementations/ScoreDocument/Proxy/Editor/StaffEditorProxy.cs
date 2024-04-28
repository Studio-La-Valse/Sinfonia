namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor;

internal class StaffEditorProxy(Staff staff) : IStaffEditor
{
    private readonly Staff staff = staff;


    public int IndexInStaffGroup => staff.IndexInStaffGroup;
}
