using System;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using PiperDotNetTts.Imp;
using DotNetTts.Helpers;
using Xunit;

namespace PiperDotNetTtsTest
{
    public class PiperTtsEngineWhould
    {
        private readonly FileInfo _piperCmd;
        private readonly DirectoryInfo _piperVoices;
        private readonly String _platform;
        private readonly DirectoryInfo _baseDir;
        public PiperTtsEngineWhould()
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
        public void PiperTtsEngineInstanceReturnInstance()
        {
            TtsEngine ttsEngine1 = PiperTtsEngine.Instance(_piperCmd, _piperVoices);
            
            Assert.IsType<PiperTtsEngine>(ttsEngine1);
        }
        
        [Fact]
        public void PiperTtsEngineInstanceWithParametersReturnOnlyInstance()
        {
            TtsEngine ttsEngine1 = PiperTtsEngine.Instance(_piperCmd, _piperVoices);
            TtsEngine ttsEngine2 = PiperTtsEngine.Instance(_piperCmd, _piperVoices);
            
            Assert.Equal(ttsEngine1, ttsEngine2);
        }

        [Fact]
        public void PiperTtsEngineInstanceReturnOnlyInstance()
        {
            TtsEngine ttsEngine1 = PiperTtsEngine.Instance(_piperCmd, _piperVoices);
            TtsEngine ttsEngine2 = PiperTtsEngine.Instance();
            
            Assert.Equal(ttsEngine1, ttsEngine2);
        }

        [Fact]
        public void PiperTtsEngineInstanceWithoutPreviosInstanceWithParametersThrowError()
        {
            Assert.Throws<TtsEngineException>(()=> PiperTtsEngine.Instance());
        }
        
        [Fact]
        public void PiperTtsEngineVoicesReturnNotNull()
        {
            TtsEngine ttsEngine1 = PiperTtsEngine.Instance(_piperCmd, _piperVoices);
            
            Assert.NotNull(ttsEngine1.Voices);
        }

        [Fact]
        public void PiperTtsEngineVoicesReturnTwoLanguages()
        {
            TtsEngine ttsEngine1 = PiperTtsEngine.Instance(_piperCmd, _piperVoices);
            
            Assert.True(ttsEngine1.Voices.Count()==2);
        }

        [Theory]
        [InlineData(null, null)]
        [InlineData(null, "noExistsDir")]
        [InlineData("noExistspiper", null)]
        [InlineData("noExistspiper", "noExistsDir")]
        public void PiperTtsEngineInstanceThrowWithNoValidEntries(String sPiperCmd, String sPiperVoices)
        {
            FileInfo piperCmd =string.IsNullOrEmpty(sPiperCmd)?null:new FileInfo(_baseDir.FullName + Path.DirectorySeparatorChar + sPiperCmd);
            DirectoryInfo piperVoices = string.IsNullOrEmpty(sPiperVoices)?null:new DirectoryInfo(_baseDir.FullName + Path.DirectorySeparatorChar + sPiperVoices);

            Assert.Throws<TtsEngineException>(()=>PiperTtsEngine.Instance(piperCmd, piperVoices));
        }
        
        [Fact]
        public void PiperTtsEngineSpeechReturnWavFile()
        {
            TtsEngine ttsEngine1 = PiperTtsEngine.Instance(_piperCmd, _piperVoices);

            var voiceInfo = ttsEngine1.Voices.FirstOrDefault(v => v.Culture.Equals(CultureInfo.GetCultureInfo("en-GB")));
            
            using TempFile f=ttsEngine1.Speech("Hello", voiceInfo);
            
            Assert.True(f!=null && f.File.Length>0);
        }
    }
}