using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using Newtonsoft.Json;

namespace DotNetTtsDeepSpeech.Imp
{
    public class PiperTtsEngine:TtsEngine
    {
        private static PiperTtsEngine _instance;
        private List<PiperVoice> _voices; 
        private readonly FileInfo _piperCmd;

        private readonly DirectoryInfo _basePiperLanguages;
        
        private PiperTtsEngine(FileInfo piperCmd, DirectoryInfo basePiperLanguages)
        {
            if (piperCmd == null || !piperCmd.Exists)
                throw new TtsEngineException($"Piper cmd path '{piperCmd}' is null or not exists.");

            if (basePiperLanguages == null || !basePiperLanguages.Exists)
                throw new TtsEngineException($"Base Piper languages path '{basePiperLanguages}' is null or not exists.");
            
            
            _piperCmd = piperCmd;
            _basePiperLanguages = basePiperLanguages;

            FindVoices();
        }
        
        public override FileInfo Speech(string text, CultureInfo culture, TtsProperties ttsProperties = null)
        {
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
            
            PiperVoice pv = _voices.FirstOrDefault(v => v.Language.Equals(culture));
            if (pv == null)
                throw new TtsEngineException($"Culture '{culture}' is not available.");

            FileInfo tmpFile = GetTempFile();

            ExecuteLinux(text, pv.LanguageFile.FullName, tmpFile.FullName);

            return tmpFile;
        }

        public override IEnumerable<CultureInfo> Languages
        {
            get
            {
                return _voices.Select(v => v.Language);
            }
        }

        private void ExecuteLinux(String text, String modelPath, String outputPath)
        {
            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = true;
                if (Environment.OSVersion.Platform.ToString()
                    .StartsWith("win", StringComparison.InvariantCultureIgnoreCase))
                {
                    p.StartInfo.FileName = "cmd";
                    String command = "echo '" + text + "' "
                                     + "| '" + _piperCmd.FullName + "'"
                                     + " --model '" + modelPath + "'"
                                     + " --output-file '" + outputPath + "'";
                    p.StartInfo.Arguments = "-c \"" + command + "\"";
                }
                else
                {
                    p.StartInfo.FileName = "sh";
                    String command = "echo '" + text + "' "
                                     + "| '" + _piperCmd.FullName + "'"
                                     + " --model '" + modelPath + "'"
                                     + " --output-file '" + outputPath + "'";
                    p.StartInfo.Arguments = "-c \"" + command + "\"";
                }

                p.Start();
                p.WaitForExit(1000);
            }
        }

        private FileInfo GetTempFile()
        {
            return new FileInfo(Path.GetTempPath() + Path.DirectorySeparatorChar + Guid.NewGuid());
        }
        private void FindVoices()
        {
            _voices = new List<PiperVoice>();
            FindVoices(_basePiperLanguages, _voices);
            
            JsonSerializer jss = JsonSerializer.Create();
            
            foreach (PiperVoice pv in _voices)
            {
                FileInfo jsonPiperVoiceFile = new FileInfo(pv.LanguageFile.FullName + ".json");
                
                using (var jstr=jsonPiperVoiceFile.OpenText())
                {
                    pv.Info=jss.Deserialize<PiperJsInfo>(new JsonTextReader(jstr));
                }

                pv.Language=CultureInfo.GetCultureInfo(pv.Info.Language.Code.Replace("_","-"));
            }
        }
        
        private void FindVoices(DirectoryInfo folder, List<PiperVoice> voices)
        {
            voices.AddRange(folder.GetFiles("*.onnx").Select(v=>new PiperVoice(){LanguageFile=v}));
            folder.GetDirectories().ToList().ForEach(d=>FindVoices(d, voices));
        }

        public static PiperTtsEngine Instance(FileInfo piperCmd, DirectoryInfo basePiperLanguages)
        {
            if (_instance == null)
                _instance = new PiperTtsEngine(piperCmd, basePiperLanguages);

            return _instance;
        }
    }
}