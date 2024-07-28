using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sinfonia.Implementations.Pipes;
internal class UndoRedoPipe : IPipe
{
    private readonly IPipe next;
    private readonly ICommandManager commandManager;
    private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;
    private bool controlDown = false;

    public UndoRedoPipe(IPipe next, ICommandManager commandManager, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged)
    {
        this.next = next;
        this.commandManager = commandManager;
        this.notifyEntityChanged = notifyEntityChanged;
    }

    public void HandleLeftMouseButtonDown()
    {
        next.HandleLeftMouseButtonDown();
    }

    public void HandleLeftMouseButtonUp()
    {
        next.HandleLeftMouseButtonUp();
    }

    public void HandleMouseWheel(double delta)
    {
        next.HandleMouseWheel(delta);
    }

    public void HandleRightMouseButtonDown()
    {
        next.HandleRightMouseButtonDown();
    }

    public void HandleRightMouseButtonUp()
    {
        next.HandleRightMouseButtonUp();
    }

    public void HandleSetMousePosition(XY position)
    {
        next.HandleSetMousePosition(position);
    }

    public void KeyDown(Key key)
    {
        if(key == Key.Control)
        {
            controlDown = true;
        }

        next.KeyDown(key);
    }

    public void KeyUp(Key key)
    {
        if (key == Key.Control)
        {
            controlDown = false;
        }

        if(controlDown && key == Key.Z)
        {
            try
            {
                commandManager.Undo();
                notifyEntityChanged.RenderChanges();
            }
            catch
            {

            }
        }

        if(controlDown && key == Key.R)
        {
            try
            {
                commandManager.Redo();
                notifyEntityChanged.RenderChanges();
            }
            catch
            {

            }
        }

        next.KeyUp(key);
    }
}
