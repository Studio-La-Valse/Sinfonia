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
    private bool controlDown = false;

    public UndoRedoPipe(IPipe next, ICommandManager commandManager)
    {
        this.next = next;
        this.commandManager = commandManager;
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
        next.HandleLeftMouseButtonUp();
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
            }
            catch
            {

            }
        }

        next.KeyUp(key);
    }
}
