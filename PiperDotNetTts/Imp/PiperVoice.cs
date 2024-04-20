using System.Globalization;
using System.IO;

namespace DotNetTtsDeepSpeech.Imp
{
    public class PiperVoice
    {
        public CultureInfo Language { get; set; }
        public FileInfo LanguageFile { get; set; }
        public PiperJsInfo Info { get; set; }
    }
}