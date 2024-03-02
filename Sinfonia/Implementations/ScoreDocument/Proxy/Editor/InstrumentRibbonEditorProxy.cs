﻿namespace Sinfonia.Implementations.ScoreDocument.Proxy.Editor
{
    internal class InstrumentRibbonEditorProxy : IInstrumentRibbonEditor, IUniqueScoreElement
    {
        private readonly InstrumentRibbon source;
        private readonly ScoreLayoutDictionary scoreLayoutDictionary;
        private readonly ICommandManager commandManager;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;




        public int IndexInScore => source.IndexInScore;

        public Instrument Instrument => source.Instrument;

        public Guid Guid => source.Guid;

        public int Id => source.Id;

        public InstrumentRibbonEditorProxy(InstrumentRibbon source, ScoreLayoutDictionary scoreLayoutDictionary, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
        {
            this.source = source;
            this.scoreLayoutDictionary = scoreLayoutDictionary;
            this.commandManager = commandManager;
            this.notifyEntityChanged = notifyEntityChanged;
        }




        public IInstrumentMeasureEditor ReadMeasure(int measureIndex)
        {
            return source.GetMeasureCore(measureIndex).ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged);
        }

        public IEnumerable<IInstrumentMeasureEditor> ReadMeasures()
        {
            return source.EnumerateMeasuresCore().Select(e => e.ProxyEditor(scoreLayoutDictionary, commandManager, notifyEntityChanged));
        }

        public IEnumerable<IScoreElement> EnumerateChildren()
        {
            return ReadMeasures();
        }
    }
}