namespace Sinfonia.Implementations.ScoreDocument.Layout
{
    /// <summary> 
    /// Defines a template property that has a field and a value. When the value is assigned a value, this value will instead be assigned to the backing field. If the field has a value assigned, this value will be used. If not, the value will be retrieved by the provided value getter.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ReferenceTemplateProperty<T> where T : class
    {
        private readonly Func<T> getDefaultValue;

        /// <summary>
        /// The backing field for this property. If it is not set, the template value will be used.
        /// </summary>
        internal T? Field { get; set; } = null;

        /// <summary>
        /// The value of the property.
        /// </summary>
        public T Value
        {
            get
            {
                if (Field is not null)
                {
                    return Field;
                }

                return getDefaultValue();
            }
            set
            {
                Field = value;
            }
        }

        /// <summary>
        /// The default constructor.
        /// </summary>
        /// <param name="getDefaultValue"></param>
        public ReferenceTemplateProperty(Func<T> getDefaultValue)
        {
            this.getDefaultValue = getDefaultValue;
        }

        /// <summary>
        /// Reset the backing field value to its default value.
        /// </summary>
        public void Reset()
        {
            Field = null;
        }
    }
}