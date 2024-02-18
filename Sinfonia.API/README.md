# Sinfonia.API

This is the main API project for the Sinfonia application. This project allows users to create and inject external addins into the application. It uses the StudioLaValse.DependencyInjection package to load the addins.

## Getting started

To get started, restore the package from a PowerShell terminal:.

```ps1
Install-Package Sinfonia.API
```

Alternatively, you can install this package using the NugetPackage manager.

There are two types of addins: scenes and external commands. Both implement the base interface IExternalAddin to store information about the addin and its creator.
To create a scene, create a class that implements the 'IExternalScene' interface.

### Creating your own scenes
```cs
public class MyScene : IExternalScene
{
	public string Name => "Scene name";
    public string Description => "My amazing scene";
    public string Author => "Who am I";
    public Guid Guid => new Guid("18A940B2-D9E1-4696-8A80-04518EB68124");

    public BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument)
    {
        var scene = ....
        return scene;
    }
}
```

Please note that the GUID is used to track the addin outside of the application lifecycle, so make sure it does not change between instantiations!

The CreateScene method will be called by the Sinfonia application, providing the addin with the score document. For more information on creating scenes, visit https://github.com/Studio-La-Valse/Drawable.

External scenes may provide some external configuration. You can configure your settings by implementing the RegisterSettings method:

```cs
public class SinglePageScene : IExternalScene
{
    private readonly IApplication application;

    public SinglePageScene(IApplication application)
    {
        this.application = application;
    }

    public override void RegisterSettings(IAddinSettingsManager animationSettingsManager)
    {
        animationSettingsManager.Register(
            () => Page,
            (val) =>
            {
                Page = val;
            },
            "The page to display");
    }

    public BaseVisualParent<IUniqueScoreElement> CreateScene(IScoreDocumentReader scoreDocument)
    {
        return new SinglePageDocumentScene(scoreDocument, () => Page);
    }
}
```

Notice how the IApplication is injected in the constructor of the scene. For now, only the IApplication from the Sinfonia.API namespace is injectable in the external scene. 
To inject your own services into the application, create a public class implementing the IExternalServiceRegistry interface from the StudioLaValse.DependencyInjection namespace.
Make sure the generic service container type is the IServiceCollection interface from the Microsoft.Extensions.DependencyInjection namespace. For example:


```
public class ExternalServiceRegistry : IExternalServiceProvider<IServiceCollection>
{
    public void AddTo(IServiceCollection container)
    {
        container.AddSingleton<IMidiService, MidiService>();
    }
}
```

For more information on the DependencyInjection extensions, visit: https://github.com/Studio-La-Valse/DependencyInjection.

### Creating externa commands
To create an external command, create a public class that implements the IExternalCommandActivator interface.

```cs
public class MyCommandActivator : IExternalCommandActivator
{
    private readonly IApplication application;

    public string Name => "My command name";
    public string Description => "This command will blow your mind.";
    public string Author => "Who am I";
    public Guid Guid => new Guid("332B2D99-7678-411C-8599-6880A839A901");

    public MyCommandActivator(IApplication application)
    {
        this.application = application;
    }

    public IExternalCommand Activate()
    {
        return new MyExternalCommand(application);
    }
}

public class MyExternalCommand : IExternalCommand
{
    private readonly IApplication application;

    public MyExternalCommand(IApplication application)
    {
        this.application = application;
    }

    public void ExecuteCommand()
    {
        var document = application.ActiveDocumentOrThrow();

        var builder = document.ScoreBuilder
            .Edit(editor =>
            {
                editor.AddInstrumentRibbon(Instrument.Violin);
            })
            .Edit(editor =>
            {
                for (int i = 0; i < 4; i++)
                {
                    editor.AppendScoreMeasure();
                }
            })
            .Build();
    }
}
```

The command activator will be a singleton, resolved at startup. The target external command will be instantiated at runtime. It will be renewed every time the command is activated by the user.

Note that the score builder has a specific set of requirements.

- Edit can be chained: they will each form one transaction which can be undone. In the above example, there are two transactions which can be undone.
- The edits (= transactions) will not be commited until .Build() is called.
- During each transaction, changed elements are tracked and invalidated by the canvas. A redraw is called at the end of the Build() action.
- You cannot edit score elements outside of the scope of an Edit action, because each modification to the score document requires an enclosing transaction to be open. If you try anyways, an InvalidOperationException will be thrown.
- The commands submitted to the transaction are executed upon enqueue, meaning you do not have to manually commit during edits. Remember however that all pending commands are queued and committed during Build().

For more information on the score document, visit: https://github.com/Studio-La-Valse/ScoreDocument
For more information on the transaction manager, visit: https://github.com/Studio-La-Valse/CommandManager

## Installing scenes and addins
To allow your scenes and addins to be found and registered by the Sinfonia application, build the .dll files and copy them to the 'External' folder which is found at the root of the directory where you installed Sinfonia.
Scenes and commands are registered during startup, meaning if you have installed new scenes or commands, you will have to restart Sinfonia.

## Contributing

Pull requests are welcome. For major changes, please open an issue first
to discuss what you would like to change.

Please make sure to update tests as appropriate.