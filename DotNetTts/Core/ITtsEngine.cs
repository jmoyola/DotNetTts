using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DotNetTts.Core
{
    public interface ITtsEngine:ITts
    {
        int Timeout { get; set; }
        IEnumerable<TtsVoiceInfo> Voices { get; }
    }
}
