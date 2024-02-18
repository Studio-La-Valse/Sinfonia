namespace Sinfonia.Implementations.ScoreDocument.Layout.Elements
{
    /// <summary>
    /// A staff system layout.
    /// </summary>
    public interface IStaffSystemLayout : IScoreElementLayout<IStaffSystemLayout>
    {
        /// <summary>
        /// Specifies how much space is reserved above the staff system, relative to the previous <see cref="IStaffSystem"/>.
        /// </summary>
        double PaddingTop { get; set; }
    }
}
