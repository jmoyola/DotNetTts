using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using PiperDotNetTts.Imp;

namespace DotNetTtsCmdTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            FileInfo piperCmd = new FileInfo("/home/jmoyola/.local/bin/piper");
            DirectoryInfo piperVoices = new DirectoryInfo("/home/jmoyola/Downloads/piper_voices");
            DirectoryInfo cachedVoices = new DirectoryInfo("/home/jmoyola/Downloads/TtsCached");
            
            TtsEngine ttsEngine = PiperTtsEngine.Instance(piperCmd, piperVoices);
            //ttsEngine = new CachedTtsEngine(ttsEngine, cachedVoices);
            
            Console.WriteLine("Languages available: " + String.Join(", ",  ttsEngine.Voices.Select(v => v.ToString())));
            Console.WriteLine("Languages available: " + ttsEngine.Speech("tudo ben", CultureInfo.GetCultureInfo("pt-BR")));
        }
    }
}