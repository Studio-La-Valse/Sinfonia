namespace Sinfonia.ViewModels
{
    public abstract class PropertyViewModel : BaseViewModel
    {
        public string Description
        {
            get => GetValue(() => Description);
            set => SetValue(() => Description, value);
        }


        public PropertyViewModel(string description)
        {
            Description = description;
        }
    }

    public class PropertyViewModel<TProperty> : PropertyViewModel
    {
        private readonly Action<TProperty> setValue;
        private readonly Func<TProperty> getValue;

        public TProperty Value
        {
            get
            {
                return getValue()!;
            }
            set
            {
                setValue(value);
                NotifyPropertyChanged(nameof(Value));
            }
        }

        public PropertyViewModel(Func<TProperty> getValue, Action<TProperty> setValue, string description) : base(description)
        {
            this.setValue = setValue;
            this.getValue = getValue;
        }
    }



    public class GenericCommandViewModel<TValue, TEntity> : PropertyViewModel where TValue : IEquatable<TValue>
    {
        private readonly IEnumerable<TEntity> entities;
        private readonly ICommandManager commandManager;
        private readonly Func<TEntity, TValue> getValue;
        private readonly Func<TEntity, TValue, BaseCommand> commandFactory;
        private readonly INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged;

        public TValue? Value
        {
            get
            {
                if (!entities.Any())
                {
                    return default;
                }

                var defaultValue = getValue(entities.First());

                if (!entities.All(v => getValue(v).Equals(defaultValue)))
                {
                    return default;
                }

                return defaultValue;
            }
            set
            {
                if (value is null)
                {
                    return;
                }

                foreach (var note in entities)
                {
                    var command = commandFactory(note, value);
                    command.Do();
                }

                //using (var transaction = commandManager.OpenTransaction(Description))
                //{
                //    foreach (var note in entities)
                //    {
                //        var command = commandFactory(note, value);
                //        transaction.Enqueue(command);
                //    }
                //}

                //notifyEntityChanged.RenderChanges();
                NotifyPropertyChanged(nameof(Value));
            }
        }

        internal GenericCommandViewModel(IEnumerable<TEntity> entities, ICommandManager commandManager, Func<TEntity, TValue> getValue, Func<TEntity, TValue, BaseCommand> commandFactory, INotifyEntityChanged<IUniqueScoreElement> notifyEntityChanged, string description) : base(description)
        {
            this.entities = entities;
            this.commandManager = commandManager;
            this.getValue = getValue;
            this.commandFactory = commandFactory;
            this.notifyEntityChanged = notifyEntityChanged;
        }
    }
}
