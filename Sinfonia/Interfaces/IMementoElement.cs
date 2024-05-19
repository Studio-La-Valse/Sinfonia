namespace Sinfonia.Interfaces;
public interface IMementoElement<TMemento>
{
    TMemento GetMemento();
    void ApplyMemento(TMemento memento);
}
