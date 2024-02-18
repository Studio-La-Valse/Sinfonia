namespace Sinfonia.Interfaces
{
    internal interface IScoreCommandFactory
    {
        BaseCommand SetNoteXOFfset(Note note, double xOffset, IUniqueScoreElement invalidate);
    }
}
