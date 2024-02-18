namespace Sinfonia.Implementations.ScoreDocument
{
    internal interface ICellFactory<TCell, TColumn, TRow>
    {
        TCell Create(TColumn column, TRow row);
    }
}
