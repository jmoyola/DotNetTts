using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using DotNetTts.Core;

namespace DotNetTts.Imp
{
    public class CachedTtsEngine:TtsEngine
    {
        private readonly TtsEngine _engine;
        private readonly DirectoryInfo _rootCacheDirectory;
        private readonly HashAlgorithm _hashAlgorithm;
        private TimeSpan _timeOfLife=TimeSpan.Zero;
        
        public CachedTtsEngine(TtsEngine engine, DirectoryInfo rootCacheDirectory)
        {
            _engine=engine??throw new ArgumentNullException(nameof(engine));
            _rootCacheDirectory=rootCacheDirectory??throw new ArgumentNullException(nameof(rootCacheDirectory));
            
            if (!rootCacheDirectory.Exists)
                Directory.CreateDirectory(rootCacheDirectory.FullName);
            
            _hashAlgorithm = HashAlgorithm.Create("SHA256");
            _hashAlgorithm?.Initialize();
        }

        public override IEnumerable<TtsVoiceInfo> Voices => _engine.Voices;

        public TimeSpan TimeOfLife
        {
            get => _timeOfLife;
            set => _timeOfLife=value;
        }

        public override FileInfo Speech(String text, TtsVoiceInfo voiceInfo, TtsProperties ttsProperties = null)
        {
            if (String.IsNullOrEmpty(text))
                throw new ArgumentNullException(nameof(text));
            
            if (voiceInfo == null)
                throw new ArgumentNullException(nameof(voiceInfo));
            
            ttsProperties= ttsProperties??TtsProperties.Default;

            DirectoryInfo bd = new DirectoryInfo(_rootCacheDirectory.FullName
                                                 + Path.DirectorySeparatorChar + voiceInfo);
            if (!bd.Exists)
                Directory.CreateDirectory(bd.FullName);
            
            String hashName=BitConverter.ToString(_hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-","");

            FileInfo cacheDestination = new FileInfo(bd.FullName + Path.DirectorySeparatorChar + hashName);
            
            if (!cacheDestination.Exists
                || (_timeOfLife>TimeSpan.Zero
                    && DateTime.UtcNow.Subtract(cacheDestination.CreationTimeUtc) >_timeOfLife))
                _engine.Speech(text, voiceInfo, cacheDestination, ttsProperties);
            
            return cacheDestination;
        }
        
        public override void Speech(String text, TtsVoiceInfo voiceInfo, FileInfo outputWavFile, TtsProperties ttsProperties=null)
        {
            if(outputWavFile==null)
                throw new ArgumentNullException(nameof(outputWavFile));
            
            FileInfo voiceFile=Speech(text, voiceInfo, ttsProperties);
            if(!voiceFile.FullName.Equals(outputWavFile.FullName))
                voiceFile.CopyTo(outputWavFile.FullName, true);
        }

        public void InvalidCache(CultureInfo culture=null)
        {
            DirectoryInfo bd;
            if(culture==null)
                bd = new DirectoryInfo(_rootCacheDirectory.FullName);
            else
                bd = new DirectoryInfo(_rootCacheDirectory.FullName
                                       + Path.DirectorySeparatorChar + (string.IsNullOrEmpty(culture.Name)?"_":culture.Name));
            
            if (bd.Exists)
                Directory.Delete(bd.FullName, true);
        }
    }
}