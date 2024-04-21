using System;
using System.Globalization;
using System.IO;
using System.Threading;

namespace DotNetTts.Core
{
    public interface ITts
    {
        void Speech(String text, CultureInfo culture, FileInfo outputWavFile, TtsProperties ttsProperties=null);
    }
}
