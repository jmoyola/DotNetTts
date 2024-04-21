using System;
using System.Collections.Generic;

namespace DotNetTts.Helpers
{
    public abstract class BaseProperties:BaseReadOnlyProperties
    {
        protected BaseProperties(IDictionary<String, Object> properties)
        :base(properties) { }
        
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