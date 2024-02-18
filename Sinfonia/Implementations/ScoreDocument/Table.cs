namespace Sinfonia.Implementations.ScoreDocument
{
    internal abstract class Table<TCell, TColumn, TRow> where TCell : class
    {
        private readonly List<(TRow header, List<TCell> cells)> rows;
        private readonly List<TColumn> columns;
        private readonly ICellFactory<TCell, TColumn, TRow> cellFactory;


        public IEnumerable<TRow> RowHeaders => rows.Select(val => val.header);
        public IEnumerable<TColumn> ColumnHeaders => columns;
        public int Width => columns.Count;
        public int Height => rows.Count;



        public Table(ICellFactory<TCell, TColumn, TRow> cellFactory)
        {
            rows = [];
            columns = [];
            this.cellFactory = cellFactory;
        }



        #region create
        public void InsertRow(TRow identifier, int index)
        {
            if (index > Height)
            {
                throw new Exception($"Cannot add a row at {index}, provide an index smaller than or equal to height {Height}");
            }

            if (RowHeaders.Contains(identifier))
            {
                throw new Exception("Row already exists in table");
            }

            var values = new List<TCell>();

            foreach (var column in columns)
            {
                var cell = cellFactory.Create(column, identifier);
                values.Add(cell);
            }

            rows.Insert(index, (identifier, values));
        }
        public void AddRow(TRow identifier)
        {
            if (RowHeaders.Contains(identifier))
            {
                throw new Exception("Row already exists in table");
            }

            var values = new List<TCell>();

            foreach (var column in columns)
            {
                var cell = cellFactory.Create(column, identifier);
                values.Add(cell);
            }

            rows.Add((identifier, values));
        }
        public void InsertColumn(TColumn identifier, int index)
        {
            if (index > Width)
            {
                throw new Exception($"Cannot add a column at {index}, provide an index smaller than or equal to width {Width}");
            }

            var existingIndex = columns.IndexOf(identifier);
            if (existingIndex != -1)
            {
                throw new Exception($"Column already exists in table, position: {existingIndex}");
            }

            columns.Insert(index, identifier);

            foreach (var (header, cells) in rows)
            {
                var cell = cellFactory.Create(identifier, header);
                cells.Insert(index, cell);
            }
        }
        public void AddColumn(TColumn identifier)
        {
            var existingIndex = columns.IndexOf(identifier);

            if (existingIndex != -1)
            {
                throw new Exception($"Column already exists in table, position: {existingIndex}");
            }

            columns.Add(identifier);

            foreach (var (header, cells) in rows)
            {
                var cell = cellFactory.Create(identifier, header);
                cells.Add(cell);
            }
        }
        #endregion


        #region read
        public TColumn ColumnAt(int i)
        {
            return columns[i];
        }
        public TRow RowAt(int i)
        {
            return rows[i].header;
        }
        public IEnumerable<TCell> GetCellsRow(int index)
        {
            return rows[index].cells;
        }
        public IEnumerable<TCell> GetCellsColumn(int index)
        {
            return rows.Select(entry => entry.cells[index]);
        }
        public int IndexOf(TColumn column)
        {
            return columns.IndexOf(column);
        }
        public int IndexOf(TRow row)
        {
            return RowHeaders.ToList().IndexOf(row);
        }
        public TCell GetCell(int x, int y)
        {
            if (!PositionExists(x, y))
            {
                throw new Exception($"Position {x}, {y} does not exist in the table");
            }

            return rows[y].cells[x];
        }
        public bool PositionExists(int x, int y)
        {
            return x >= 0 && y >= 0 && x < Width && y < Height;
        }
        #endregion



        #region delete
        public void RemoveRow(int index)
        {
            if (index < 0 || index > Height)
                throw new Exception("Index is out of range");

            rows.RemoveAt(index);
        }
        public void RemoveColumn(int index)
        {
            columns.RemoveAt(index);
            foreach (var entry in rows)
            {
                entry.cells.RemoveAt(index);
            }
        }
        #endregion
    }
}
