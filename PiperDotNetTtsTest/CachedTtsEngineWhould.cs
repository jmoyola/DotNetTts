using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using DotNetTts.Imp;
using PiperDotNetTts.Imp;
using DotNetTts.Helpers;
using Xunit;

namespace PiperDotNetTtsTest
{
    public class CachedTtsEngineWhould
    {
        private readonly FileInfo _piperCmd;
        private readonly DirectoryInfo _piperVoices;
        private readonly String _platform;
        private readonly DirectoryInfo _baseDir;
        
        public CachedTtsEngineWhould()
        {
            _platform =
                Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.InvariantCultureIgnoreCase)
                    ? "Win"
                    : "Linux";

            
                
            _baseDir = new DirectoryInfo(Directory.GetCurrentDirectory());
            _piperCmd =new FileInfo(_baseDir.FullName
                + Path.DirectorySeparatorChar + "resources"
                + Path.DirectorySeparatorChar + "bin"
                + Path.DirectorySeparatorChar + _platform
                + Path.DirectorySeparatorChar + "piper"
                + Path.DirectorySeparatorChar + "piper" + (_platform.Equals("Win")?".exe":null)
            );
            
            _piperVoices = new DirectoryInfo(_baseDir.FullName
                + Path.DirectorySeparatorChar + "resources"
                + Path.DirectorySeparatorChar + "voices"
            );
        }
        
        [Fact]
        public void CachedTtsEngineVoicesReturnNotNull()
        {
            using TempDirectory cacheBaseDir = TempDirectory.Create();
            
            TtsEngine ttsEngine1 =new CachedTtsEngine(PiperTtsEngine.Instance(_piperCmd, _piperVoices), cacheBaseDir);
            
            Assert.NotNull(ttsEngine1.Voices);
        }

        [Fact]
        public void CachedTtsEngineVoicesReturnTwoLanguages()
        {
            using TempDirectory cacheBaseDir = TempDirectory.Create();
            
            TtsEngine ttsEngine1 =new CachedTtsEngine(PiperTtsEngine.Instance(_piperCmd, _piperVoices), cacheBaseDir);
            
            Assert.True(ttsEngine1.Voices.Count()==2);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "noExistsDir")]
        [InlineData("noExistspiper", null)]
        [InlineData("noExistspiper", "noExistsDir")]
        public void CachedTtsEngineInstanceThrowWithNoValidEntries(String sPiperCmd, String sPiperVoices)
        {
            using TempDirectory cacheBaseDir = TempDirectory.Create();
            
            FileInfo piperCmd =string.IsNullOrEmpty(sPiperCmd)?null:new FileInfo(_baseDir.FullName + Path.DirectorySeparatorChar + sPiperCmd);
            DirectoryInfo piperVoices = string.IsNullOrEmpty(sPiperVoices)?null:new DirectoryInfo(_baseDir.FullName + Path.DirectorySeparatorChar + sPiperVoices);

            Assert.Throws<TtsEngineException>(()=>new CachedTtsEngine(PiperTtsEngine.Instance(piperCmd, piperVoices), cacheBaseDir));
        }
        
        [Fact]
        public void CachedTtsEngineSpeechReturnSameWavFile()
        {
            using TempDirectory cacheBaseDir = TempDirectory.Create();
            
            TtsEngine ttsEngine1 =new CachedTtsEngine(PiperTtsEngine.Instance(_piperCmd, _piperVoices), cacheBaseDir);
            
            var voiceInfo = ttsEngine1.Voices.FirstOrDefault(v => v.Culture.Equals(CultureInfo.GetCultureInfo("en-GB")));
            
            TempFile f1=ttsEngine1.Speech("Hello",voiceInfo);
            TempFile f2=ttsEngine1.Speech("Hello", voiceInfo);
            
            Assert.True(f1.Equals(f2));
        }
    }
}