namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class StaffSystemEditorProxy : IStaffSystemEditor
    {
        private readonly StaffSystem staffSystem;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;




        public StaffSystemEditorProxy(StaffSystem staffSystem, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.staffSystem = staffSystem;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public IEnumerable<IScoreMeasureEditor> EnumerateMeasures()
        {
            return staffSystem.EnumerateMeasures().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IEnumerable<IStaffGroupEditor> EnumerateStaffGroups()
        {
            return staffSystem.EnumerateStaffGroups().Select(e => e.ProxyEditor(commandManager, notifyEntityChanged));
        }

        public IStaffSystemLayout ReadLayout()
        {
            return staffSystem.Layout;
        }
    }
}
