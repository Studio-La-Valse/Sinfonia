namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class StaffSystemEditorProxy : IStaffSystemEditor, IUniqueScoreElement
    {
        private readonly StaffSystem staffSystem;
        private readonly ScoreLayoutDictionary scoreLayoutDictionary;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;



        public Guid Guid => staffSystem.Guid;

        public int Id => staffSystem.Id;

        public ScoreDocumentCore HostScoreDocument => staffSystem.HostScoreDocument;


        public StaffSystemEditorProxy(StaffSystem staffSystem, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.staffSystem = staffSystem;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public IEnumerable<IScoreMeasureEditor> EnumerateMeasures()
        {
            return staffSystem.EnumerateMeasures().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
        }

        public IEnumerable<IStaffGroupEditor> EnumerateStaffGroups()
        {
            return staffSystem.EnumerateStaffGroups().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            foreach (IScoreMeasureEditor measure in EnumerateMeasures())
            {
                yield return measure;
            }

            foreach (IStaffGroupEditor staffGroup in EnumerateStaffGroups())
            {
                yield return staffGroup;
            }
        }

        public StaffSystemLayout ReadLayout()
        {
            return scoreLayoutDictionary.StaffSystemLayout(this);
        }

        public void Apply(StaffSystemLayout layout)
        {
            scoreLayoutDictionary.Apply(this, layout);
        }

        public void RemoveLayout()
        {
            scoreLayoutDictionary.Restore(this);
        }
    }
}
