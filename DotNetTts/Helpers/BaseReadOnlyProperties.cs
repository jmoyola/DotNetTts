using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
        protected readonly IDictionary<String, Object> InternalProperties;
        protected BaseReadOnlyProperties(IDictionary<String, Object> properties)
        {
            InternalProperties = properties ?? throw new ArgumentNullException(nameof(properties));
        }

        public bool TryGetValue<T>(String key, out T output)
        {
            output = default(T);
            
            if (InternalProperties.ContainsKey(key))
            {
                output = (T) InternalProperties[key];
                return true;
            }

            return false;
        }

        public T GetValue<T>(String key)
        {
            return (T) InternalProperties[key];
        }

        public IReadOnlyList<String> Keys =>
            InternalProperties.Keys.ToList().AsReadOnly();
        
        public Type GetType(String key)
        {
            return InternalProperties[key].GetType();
        }
        
        public bool ContainsKey(String key)
        {
            return InternalProperties.ContainsKey(key);
        }

    }
}