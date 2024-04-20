using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace DotNetTts.Core
{
    public abstract class Tts
    {
        public FileInfo Speech(String text, TtsProperties ttsProperties = null)
        {
            return this.Speech(text, Thread.CurrentThread.CurrentCulture, ttsProperties);
        }
        public abstract FileInfo Speech(String text, CultureInfo culture, TtsProperties ttsProperties=null);
    }
}
