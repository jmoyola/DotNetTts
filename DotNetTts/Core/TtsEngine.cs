using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DotNetTts.Core
{
    public abstract class TtsEngine:Tts, ITtsEngine
    {
        public int Timeout { get; set; } = 0;
        public abstract IEnumerable<TtsVoiceInfo> Voices { get; }
    }
}
