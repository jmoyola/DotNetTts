using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using DotNetTts.Helpers;

namespace DotNetTts.Core
{
    public abstract class TtsEngine: ITtsEngine
    {
        public int Timeout { get; set; } = 0;
        
        public FileInfo Speech(String text, TtsProperties ttsProperties = null)
        {
            TtsVoiceInfo voiceInfo =
                this.Voices.FirstOrDefault(v => v.Culture.Equals(Thread.CurrentThread.CurrentCulture));
            
            return this.Speech(text, voiceInfo, ttsProperties);
        }

        public virtual FileInfo Speech(String text, TtsVoiceInfo voiceInfo, TtsProperties ttsProperties = null)
        {
            TempFile tempFile = TempFile.Create();
            Speech(text, voiceInfo, tempFile, ttsProperties);
            return tempFile;
        }
        
        public abstract void Speech(String text, TtsVoiceInfo voiceInfo, FileInfo outputWavFile, TtsProperties ttsProperties=null);
        
        public abstract IEnumerable<TtsVoiceInfo> Voices { get; }
    }
}
