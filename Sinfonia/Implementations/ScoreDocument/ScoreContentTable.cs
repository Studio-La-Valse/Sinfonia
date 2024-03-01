namespace Sinfonia.Implementations.ScoreDocument
{
    internal sealed class ScoreContentTable
    {
        private readonly List<(InstrumentRibbon instrumentRibbon, List<InstrumentMeasure> instrumentMeasures)> instrumentRibbons;
        private readonly List<ScoreMeasure> scoreMeasures;
        private readonly InstrumentMeasureFactory cellFactory;


        public IEnumerable<InstrumentRibbon> RowHeaders => instrumentRibbons.Select(val => val.instrumentRibbon);
        public IEnumerable<ScoreMeasure> ColumnHeaders => scoreMeasures;
        public int Width => scoreMeasures.Count;
        public int Height => instrumentRibbons.Count;



        public ScoreContentTable(InstrumentMeasureFactory cellFactory)
        {
            this.cellFactory = cellFactory;

            instrumentRibbons = [];
            scoreMeasures = [];
        }



        #region create
        public void InsertInstrumentRibbon(InstrumentRibbon identifier, int index)
        {
            if (index > Height)
            {
                throw new Exception($"Cannot add a row at {index}, provide an index smaller than or equal to height {Height}");
            }

            if (RowHeaders.Contains(identifier))
            {
                throw new Exception("Row already exists in table");
            }

            var values = new List<InstrumentMeasure>();

            foreach (var column in scoreMeasures)
            {
                var cell = cellFactory.Create(column, identifier);
                values.Add(cell);
            }

            instrumentRibbons.Insert(index, (identifier, values));
        }
        public void AddInstrumentRibbon(InstrumentRibbon identifier)
        {
            if (RowHeaders.Contains(identifier))
            {
                throw new Exception("Row already exists in table");
            }

            var values = new List<InstrumentMeasure>();

            foreach (var column in scoreMeasures)
            {
                var cell = cellFactory.Create(column, identifier);
                values.Add(cell);
            }

            instrumentRibbons.Add((identifier, values));
        }
        public void InsertScoreMeasure(ScoreMeasure identifier, int index)
        {
            if (index > Width)
            {
                throw new Exception($"Cannot add a column at {index}, provide an index smaller than or equal to width {Width}");
            }

            var existingIndex = scoreMeasures.IndexOf(identifier);
            if (existingIndex != -1)
            {
                throw new Exception($"Column already exists in table, position: {existingIndex}");
            }

            scoreMeasures.Insert(index, identifier);

            foreach (var (header, cells) in instrumentRibbons)
            {
                var cell = cellFactory.Create(identifier, header);
                cells.Insert(index, cell);
            }
        }
        public void AddScoreMeasure(ScoreMeasure identifier)
        {
            var existingIndex = scoreMeasures.IndexOf(identifier);

            if (existingIndex != -1)
            {
                throw new Exception($"Column already exists in table, position: {existingIndex}");
            }

            scoreMeasures.Add(identifier);

            foreach (var (header, cells) in instrumentRibbons)
            {
                var cell = cellFactory.Create(identifier, header);
                cells.Add(cell);
            }
        }
        #endregion


        #region read
        public ScoreMeasure ScoreMeasureAt(int i)
        {
            return scoreMeasures[i];
        }
        public InstrumentRibbon InstrumentRibbonAt(int i)
        {
            return instrumentRibbons[i].instrumentRibbon;
        }
        public IEnumerable<InstrumentMeasure> GetInstrumentMeasuresInInstrumentRibbon(int index)
        {
            return instrumentRibbons[index].instrumentMeasures;
        }
        public IEnumerable<InstrumentMeasure> GetInstrumentMeasuresInScoreMeasure(int index)
        {
            return instrumentRibbons.Select(entry => entry.instrumentMeasures[index]);
        }
        public int IndexOf(ScoreMeasure column)
        {
            return scoreMeasures.IndexOf(column);
        }
        public int IndexOf(InstrumentRibbon row)
        {
            return RowHeaders.ToList().IndexOf(row);
        }
        public InstrumentMeasure GetInstrumentMeasure(int x, int y)
        {
            if (!PositionExists(x, y))
            {
                throw new Exception($"Position {x}, {y} does not exist in the table");
            }

            return instrumentRibbons[y].instrumentMeasures[x];
        }
        public bool PositionExists(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
        #endregion



        #region delete
        public void RemoveInstrumentRibbon(int index)
        {
            if (index < 0 || index > Height)
                throw new Exception("Index is out of range");

            instrumentRibbons.RemoveAt(index);
        }
        public void RemoveScoreMeasure(int index)
        {
            scoreMeasures.RemoveAt(index);
            foreach (var entry in instrumentRibbons)
            {
                entry.instrumentMeasures.RemoveAt(index);
            }
        }
        #endregion
    }
}
