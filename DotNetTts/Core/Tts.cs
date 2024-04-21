using System;
using System.Globalization;
using System.IO;
using System.Threading;
using DotNetTts.Helpers;

namespace DotNetTts.Core
{
    public abstract class Tts:ITts
    {
        public FileInfo Speech(String text, TtsProperties ttsProperties = null)
        {
            return this.Speech(text, Thread.CurrentThread.CurrentCulture, ttsProperties);
        }

        public virtual FileInfo Speech(String text, CultureInfo culture, TtsProperties ttsProperties = null)
        {
            TempFile tempFile = TempFile.Create();
            Speech(text, culture, tempFile, ttsProperties);
            return tempFile;
        }
        
        public abstract void Speech(String text, CultureInfo culture, FileInfo outputWavFile, TtsProperties ttsProperties=null);
    }
}
