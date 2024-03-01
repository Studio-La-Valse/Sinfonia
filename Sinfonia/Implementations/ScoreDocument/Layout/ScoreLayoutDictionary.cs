using StudioLaValse.ScoreDocument.Layout.ScoreElements;

namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    public class ScoreLayoutDictionary : StudioLaValse.ScoreDocument.Layout.IScoreLayoutDictionary, StudioLaValse.ScoreDocument.Builder.IScoreLayoutDictionary
    {
        private readonly Dictionary<Guid, NoteLayout> measureElementLayoutDictionary = [];
        private readonly Dictionary<Guid, ChordLayout> chordLayoutDictionary = [];
        private readonly Dictionary<Guid, MeasureBlockLayout> measureBlockLayoutDictionary = [];

        private readonly Dictionary<Guid, InstrumentMeasureLayout> instrumentMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, ScoreMeasureLayout> scoreMeasureLayoutDictionary = [];
        private readonly Dictionary<Guid, InstrumentRibbonLayout> instrumentRibbonLayoutDictionary = [];

        private readonly Dictionary<Guid, StaffLayout> staffLayoutDictionary = [];
        private readonly Dictionary<Guid, StaffGroupLayout> staffGroupLayoutDictionary = [];
        private readonly Dictionary<Guid, StaffSystemLayout> staffSystemLayoutDictionary = [];
        private readonly ScoreDocumentStyle documentStyle;
        private readonly StaffSystemGenerator staffSystemGenerator;
        private ScoreDocumentLayout? documentLayout;


        public ScoreLayoutDictionary(ScoreDocumentStyle documentStyle, StaffSystemGenerator staffSystemGenerator)
        {
            this.documentStyle = documentStyle;
            this.staffSystemGenerator = staffSystemGenerator;
        }


        public ScoreDocumentLayout GetOrDefault(IScoreDocument document)
        {
            if (documentLayout is null)
            {
                return documentStyle.ScoreDocumentLayoutFactory(document);
            }

            return documentLayout.Copy();
        }
        public void Apply(IScoreDocument scoreDocument, ScoreDocumentLayout layout)
        {
            documentLayout = layout;
        }


        public NoteLayout GetOrDefault(INote note)
        {
            if (measureElementLayoutDictionary.TryGetValue(note.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.NoteLayoutFactory(note);
        }
        public void Apply(INote note, NoteLayout layout)
        {
            measureElementLayoutDictionary[note.Guid] = layout;
        }


        public ChordLayout GetOrDefault(IChord chord)
        {
            if (chordLayoutDictionary.TryGetValue(chord.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.ChordLayoutFactory(chord);
        }
        public void Apply(IChord chord, ChordLayout layout)
        {
            chordLayoutDictionary[chord.Guid] = layout;
        }


        public MeasureBlockLayout GetOrDefault(IMeasureBlock chord)
        {
            if (measureBlockLayoutDictionary.TryGetValue(chord.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.MeasureBlockStyle(chord);
        }
        public void Apply(IMeasureBlock measureBlock, MeasureBlockLayout layout)
        {
            measureBlockLayoutDictionary[measureBlock.Guid] = layout;
        }


        public InstrumentMeasureLayout GetOrDefault(IInstrumentMeasure instrumentMeasure)
        {
            if (instrumentMeasureLayoutDictionary.TryGetValue(instrumentMeasure.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.InstrumentMeasureLayoutFactory(instrumentMeasure);
        }
        public void Apply(IInstrumentMeasure instrumentMeasure, InstrumentMeasureLayout layout)
        {
            instrumentMeasureLayoutDictionary[instrumentMeasure.Guid] = layout;
        }


        public ScoreMeasureLayout GetOrDefault(IScoreMeasure scoreMeasure)
        {
            if (scoreMeasureLayoutDictionary.TryGetValue(scoreMeasure.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.ScoreMeasureLayoutFactory(scoreMeasure);
        }
        public void Apply(IScoreMeasure scoreMeasure, ScoreMeasureLayout layout)
        {
            scoreMeasureLayoutDictionary[scoreMeasure.Guid] = layout;
        }


        public InstrumentRibbonLayout GetOrDefault(IInstrumentRibbon instrumentRibbon)
        {
            if (instrumentRibbonLayoutDictionary.TryGetValue(instrumentRibbon.Guid, out var value))
            {
                return value.Copy();
            }

            return documentStyle.InstrumentRibbonLayoutFactory(instrumentRibbon);
        }
        public void Apply(IInstrumentRibbon instrumentRibbon, InstrumentRibbonLayout layout)
        {
            instrumentRibbonLayoutDictionary[instrumentRibbon.Guid] = layout;
        }


        public StaffLayout GetOrDefault(IStaff staff)
        {
            if (staffLayoutDictionary.TryGetValue(staff.Guid, out var layout))
            {
                return layout.Copy();
            }

            return documentStyle.StaffLayoutFactory(staff);
        }
        public void Apply(IStaff staff, StaffLayout layout)
        {
            staffLayoutDictionary[staff.Guid] = layout;
        }


        public StaffGroupLayout GetOrDefault(IStaffGroup staffGroup)
        {
            if (staffGroupLayoutDictionary.TryGetValue(staffGroup.Guid, out var layout))
            {
                return layout.Copy();
            }

            return documentStyle.StaffGroupLayoutFactory(staffGroup);
        }
        public void Apply(IStaffGroup staffGroup, StaffGroupLayout layout)
        {
            staffGroupLayoutDictionary[staffGroup.Guid] = layout;
        }


        public StaffSystemLayout GetOrDefault(IStaffSystem staffSystem)
        {
            if (staffSystemLayoutDictionary.TryGetValue(staffSystem.Guid, out var layout))
            {
                return layout.Copy();
            }

            return documentStyle.StaffSystemLayoutFactory(staffSystem);
        }
        public void Apply(IStaffSystem staffSystem, StaffSystemLayout layout)
        {
            staffSystemLayoutDictionary[staffSystem.Guid] = layout;
        }

        public IEnumerable<IStaffSystem> EnumerateStaffSystems(IScoreDocumentReader scoreDocument)
        {
            var layout = GetOrDefault(scoreDocument);
            return staffSystemGenerator.EnumerateStaffSystems(scoreDocument, layout);
        }
    }
}
