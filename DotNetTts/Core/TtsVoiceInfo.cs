using System;
using System.Collections.Generic;
using System.Globalization;
using DotNetTts.Helpers;

namespace DotNetTts.Core
{
    public class TtsVoiceInfo:BaseReadOnlyProperties
    {
        public TtsVoiceInfo(string id, string name, CultureInfo culture)
        :base(new Dictionary<string, object>())
        {
            if (String.IsNullOrEmpty(id))
                throw new ArgumentNullException(nameof(id));

            if (String.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));

            if (culture==null)
                throw new ArgumentNullException(nameof(culture));

            Properties.Add("id", id);
            Properties.Add("name", name);
            Properties.Add("culture", culture);
        }
        
        public string Id => this.GetValue<String>("id");
        
        public string Name => this.GetValue<String>("name");
        
        public CultureInfo Culture=>this.GetValue<CultureInfo>("culture");
        

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
            return $"{Id} {Culture.Name} {Name}";
        }
    }
}