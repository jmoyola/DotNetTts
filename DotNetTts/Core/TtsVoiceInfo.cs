using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetTts.Helpers;

namespace DotNetTts.Core
{
    public class TtsVoiceInfo:BaseReadOnlyProperties
    {
        public TtsVoiceInfo(string path, CultureInfo culture, string name="default", string quality="default")
        :base(new Dictionary<string, object>())
        {
            if (String.IsNullOrEmpty(path))
                throw new ArgumentNullException(nameof(path));

            if (culture==null)
                throw new ArgumentNullException(nameof(culture));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (String.IsNullOrEmpty(quality))
                throw new ArgumentNullException(nameof(quality));
            
            InternalProperties.Add("path", path);
            InternalProperties.Add("culture", culture);
            InternalProperties.Add("name", name);
            InternalProperties.Add("quality", quality);
        }

        public string Id => Culture.Name.Replace("-", "_") + "-" + Name + "-" + Quality;
        
        public string Path => this.GetValue<String>("path");
        
        public CultureInfo Culture=>this.GetValue<CultureInfo>("culture");
        
        public string Name => this.GetValue<String>("name");
        
        public string Quality => this.GetValue<String>("quality");
        
        public override bool Equals(object obj)
        {
            return obj is TtsVoiceInfo info && info.Id== this.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString()
        {
            return $"{Id} ({Path})";
        }
    }
}