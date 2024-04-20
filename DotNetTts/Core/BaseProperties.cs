using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetTts.Core
{
    [Serializable]
    public class TtsPropertiesException:Exception
    {
        protected TtsPropertiesException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TtsPropertiesException(string message) : base(message)
        {
        }

        public TtsPropertiesException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
    
    public abstract class BaseProperties
    {
        protected readonly IDictionary<String, Object> Properties;
        protected BaseProperties(IDictionary<String, Object> properties)
        {
            Properties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public bool TryGetValue<T>(String key, out T output)
        {
            output = default(T);
            
            if (Properties.ContainsKey(key))
            {
                output = (T) Properties[key];
                return true;
            }

            return false;
        }

        public T GetValue<T>(String key, T defaultValueIfNotExists)
        {
            if (Properties.ContainsKey(key))
                return (T) Properties[key];
            else
                return defaultValueIfNotExists;
        }

        public bool SetValue(String key, Object value)
        {
            if (!Properties.ContainsKey(key))
                throw new TtsPropertiesException($"Property with name '{key}' don't exists.");
            
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            Type t = Properties[key].GetType();
            try
            {
                Properties[key] = Convert.ChangeType(value, t);
            }
            catch(Exception ex)
            {
                throw new TtsPropertiesException($"Can't set value of type '{value.GetType()}' to property with name '{key}' ({t}).", ex);
            }

            return true;
        }
    }
}