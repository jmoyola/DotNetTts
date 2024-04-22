using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace DotNetTts.Core
{
    public interface ITtsEngine
    {
        int Timeout { get; set; }
        void Speech(String text, TtsVoiceInfo voiceInfo, FileInfo outputWavFile, TtsProperties ttsProperties=null);
        IEnumerable<TtsVoiceInfo> Voices { get; }
    }
}
