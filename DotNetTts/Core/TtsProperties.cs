using System.Collections.Generic;
using DotNetTts.Helpers;

namespace DotNetTts.Core
{
    public class TtsProperties:BaseProperties
    {
        private static TtsProperties _default;

        public TtsProperties()
            :base(new Dictionary<string, object>())
        { }

        public static TtsProperties Default
        {
            get
            {
                if(_default==null)
                    _default = new TtsProperties();

                return _default;
            }
        }
    }
}