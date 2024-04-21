using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DotNetTts.Helpers
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
    
    public abstract class BaseReadOnlyProperties
    {
        protected readonly IDictionary<String, Object> Properties;
        protected BaseReadOnlyProperties(IDictionary<String, Object> properties)
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

        public T GetValue<T>(String key)
        {
            return (T) Properties[key];
        }
    }
}