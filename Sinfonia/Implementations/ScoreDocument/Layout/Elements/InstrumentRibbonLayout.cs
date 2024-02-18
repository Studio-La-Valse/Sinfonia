namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// The layout of an instrument ribbon.
    /// </summary>
    public class InstrumentRibbonLayout : IInstrumentRibbonLayout
    {
        /// <inheritdoc/>
        public string AbbreviatedName { get; set; }
        /// <inheritdoc/>
        public bool Collapsed { get; set; }
        /// <inheritdoc/>
        public string DisplayName { get; set; }
        /// <inheritdoc/>
        public int NumberOfStaves { get; set; }


        /// <summary>
        /// Create a new instrument ribbon layout.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="abbreviatedname"></param>
        /// <param name="numberOfStaves"></param>
        /// <param name="collapsed"></param>
        public InstrumentRibbonLayout(string name, string abbreviatedname, int numberOfStaves, bool collapsed = false)
        {
            DisplayName = name;
            AbbreviatedName = abbreviatedname;
            NumberOfStaves = numberOfStaves;
            Collapsed = collapsed;
        }
        /// <summary>
        /// Create a new instrument ribbon layout.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="numberOfStaves"></param>
        /// <param name="collapsed"></param>
        public InstrumentRibbonLayout(string name, int numberOfStaves, bool collapsed = false) : this(name, CreateDefaultNickName(name), numberOfStaves, collapsed)
        {

        }
        /// <summary>
        /// Create a new instrument ribbon layout.
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="collapsed"></param>
        public InstrumentRibbonLayout(Instrument instrument, bool collapsed = false) : this(instrument.Name, instrument.NumberOfStaves, collapsed)
        {

        }
        /// <summary>
        /// Create a new instrument ribbon layout.
        /// </summary>
        /// <param name="instrument"></param>
        /// <param name="nickname"></param>
        /// <param name="collapsed"></param>
        public InstrumentRibbonLayout(Instrument instrument, string nickname, bool collapsed = false) : this(instrument.Name, nickname, instrument.NumberOfStaves, collapsed)
        {

        }

        /// <summary>
        /// Create the default nickname.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CreateDefaultNickName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return "";
            }
            else if (name.Length == 1)
            {
                return string.Concat(name.AsSpan(0, 1), ".");
            }
            else
            {
                return string.Concat(name.AsSpan(0, 2), ".");
            }
        }

        /// <inheritdoc/>
        public IInstrumentRibbonLayout Copy()
        {
            return new InstrumentRibbonLayout(DisplayName, AbbreviatedName, NumberOfStaves, Collapsed);
        }
    }
}