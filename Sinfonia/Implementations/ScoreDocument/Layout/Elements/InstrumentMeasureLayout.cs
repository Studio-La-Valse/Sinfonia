namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// Represents an instrument measure layout.
    /// </summary>
    public class InstrumentMeasureLayout : IInstrumentMeasureLayout
    {
        private readonly HashSet<ClefChange> _changeList = [];

        /// <inheritdoc/>
        public IEnumerable<ClefChange> ClefChanges => _changeList;

        /// <summary>
        /// Create a new Instrument measure layout.
        /// </summary>
        public InstrumentMeasureLayout()
        {

        }

        /// <inheritdoc/>
        public void AddClefChange(ClefChange clefChange)
        {
            _changeList.Add(clefChange);
        }

        /// <inheritdoc/>
        public void RemoveClefChange(ClefChange clefChange)
        {
            _changeList.Remove(clefChange);
        }

        /// <inheritdoc/>
        public IInstrumentMeasureLayout Copy()
        {
            var layout = new InstrumentMeasureLayout();
            foreach (var change in _changeList)
            {
                layout.AddClefChange(change);
            }
            return layout;
        }
    }
}