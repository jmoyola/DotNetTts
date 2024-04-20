using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DotNetTts.Core
{
    public abstract class TtsEngine:Tts
    {
        public abstract IEnumerable<CultureInfo> Languages { get; }
    }
}
