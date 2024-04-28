using System;
using System.Collections.Generic;

namespace DotNetTts.Helpers
{
    public class BaseProperties:BaseReadOnlyProperties
    {
        public BaseProperties(IDictionary<String, Object> properties)
        :base(properties) { }
        
        public void SetValue(String key, Object value)
        {
            if (!InternalProperties.ContainsKey(key))
                throw new TtsPropertiesException($"Property with name '{key}' don't exists.");
            
            if (value == null)
                throw new ArgumentNullException(nameof(value));
            
            Type t = InternalProperties[key].GetType();
            try
            {
                InternalProperties[key] = Convert.ChangeType(value, t);
            }
            catch(Exception ex)
            {
                throw new TtsPropertiesException($"Can't set value of type '{value.GetType()}' to property with name '{key}' ({t}).", ex);
            }
        }
    }
}