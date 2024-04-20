using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DotNetTts.Core
{
    public class CachedTtsEngine:TtsEngine
    {
        private readonly TtsEngine _engine;
        private readonly DirectoryInfo _rootCacheDirectory;
        private HashAlgorithm h = HashAlgorithm.Create("SHA256");
        
        
        public CachedTtsEngine(TtsEngine engine, DirectoryInfo rootCacheDirectory)
        {
            if (engine == null)
                throw new ArgumentNullException(nameof(engine));
            
            if (rootCacheDirectory == null)
                throw new ArgumentNullException(nameof(rootCacheDirectory));

            _engine = engine;
            _rootCacheDirectory = rootCacheDirectory;
            
            if (!rootCacheDirectory.Exists)
                Directory.CreateDirectory(rootCacheDirectory.FullName);
            
            h.Initialize();
        }

        public override IEnumerable<CultureInfo> Languages => _engine.Languages;

        public override FileInfo Speech(String text, CultureInfo culture, TtsProperties ttsProperties=null)
        {
            if (culture==null)
                culture = CultureInfo.InvariantCulture;
            
            ttsProperties= ttsProperties??TtsProperties.Default;

            DirectoryInfo bd = new DirectoryInfo(_rootCacheDirectory.FullName
                               + Path.DirectorySeparatorChar + culture.Name);
            if (!bd.Exists)
                Directory.CreateDirectory(bd.FullName);
            
            
            String sh=BitConverter.ToString(h.ComputeHash(Encoding.UTF8.GetBytes(text))).Replace("-","");

            FileInfo destination = new FileInfo(bd.FullName + Path.DirectorySeparatorChar + sh);
            if (!destination.Exists)
            {
                FileInfo source = _engine.Speech(text, culture, ttsProperties);
                source.MoveTo(destination.FullName);
                destination.Refresh();
            }

            return destination;
        }
    }
}