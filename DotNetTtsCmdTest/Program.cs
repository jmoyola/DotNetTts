using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using DotNetTts.Core;
using PiperDotNetTts.Imp;

namespace DotNetTtsCmdTest
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            FileInfo piperCmd;
            DirectoryInfo piperHome;
            DirectoryInfo piperVoices;
            DirectoryInfo cachedVoices;

            DirectoryInfo userHome=new DirectoryInfo(Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.CurrentCultureIgnoreCase)?
                Environment.ExpandEnvironmentVariables("%HOMEDRIVE%%HOMEPATH%"):
                Environment.GetEnvironmentVariable("$HOME")
                );

            piperHome= new DirectoryInfo(userHome.FullName
                           + Path.DirectorySeparatorChar + "software"
                           + Path.DirectorySeparatorChar + "piper"
            );
            
            if (Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.CurrentCultureIgnoreCase))
            {
                piperCmd = new FileInfo(piperHome.FullName
                    + Path.DirectorySeparatorChar + "binaries"
                    + Path.DirectorySeparatorChar + "Win_" + RuntimeInformation.OSArchitecture
                    + Path.DirectorySeparatorChar + "piper.exe"
                );
            }
            else
            {
                piperCmd = new FileInfo(piperHome.FullName
                    + Path.DirectorySeparatorChar + "binaries"
                    + Path.DirectorySeparatorChar + "Linux_" + RuntimeInformation.OSArchitecture
                    + Path.DirectorySeparatorChar + "piper"
                );            }

            piperVoices = new DirectoryInfo(piperHome.FullName
                                            + Path.DirectorySeparatorChar + "voices"
            );
                
            cachedVoices = new DirectoryInfo(userHome.FullName
                                             + Path.DirectorySeparatorChar + "ttsCache"
            );

            TtsEngine ttsEngine;
            ttsEngine = PiperTtsEngine.Instance(piperVoices);
            //ttsEngine = PiperTtsEngine.Instance(piperCmd, piperVoices);
            //ttsEngine = new CachedTtsEngine(ttsEngine, cachedVoices);

            var voices = ttsEngine.Voices;
            Console.WriteLine("Languages available: " + String.Join(", ",  voices.Select(v => v.ToString())));
            var voiceInfo = voices.FirstOrDefault(v => v.Culture.Equals(CultureInfo.GetCultureInfo("pt-BR")));
            Console.WriteLine("Languages available: " + ttsEngine.Speech("tudo ben", voiceInfo));
        }
    }
}