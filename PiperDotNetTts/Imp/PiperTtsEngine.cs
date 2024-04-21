using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using Newtonsoft.Json;

namespace PiperDotNetTts.Imp
{
    public class PiperTtsEngine:TtsEngine
    {
        private static TtsEngine _instance;
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
        
        public override void Speech(string text, CultureInfo culture, FileInfo outputWavFile, TtsProperties ttsProperties = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            
            if (culture == null)
                throw new ArgumentNullException(nameof(culture));
            
            if (outputWavFile == null)
                throw new ArgumentNullException(nameof(outputWavFile));
            
            PiperVoice pv = _voices.Find(v => v.Language.Equals(culture));
            if (pv == null)
                throw new TtsEngineException($"Culture '{culture}' is not available.");

            ExecutePiper(text, pv.LanguageFile, outputWavFile);
        }

        public override IEnumerable<TtsVoiceInfo> Voices
        {
            get
            {
                return _voices.Select(v => new TtsVoiceInfo(v.LanguageFile.Name, v.Info.Dataset, v.Language));
            }
        }

        private void ExecutePiper(String text, FileInfo modelPath, FileInfo outputPath)
        {
            int exitCode;
            string command;
            if (Environment.OSVersion.Platform.ToString()
                .StartsWith("win", StringComparison.InvariantCultureIgnoreCase))
            {
                command="echo '" + text + "'"
                                     + " | '" + _piperCmd.FullName + "'"
                                     + " --model '" + modelPath + "'"
                                     + " --output-file '" + outputPath + "'";
            }
            else
            {
                command="echo '" + text + "'"
                                     + " | '" + _piperCmd.FullName + "'"
                                     + " --model '" + modelPath + "'"
                                     + " --output-file '" + outputPath + "'";
            }

            exitCode = ExecuteShell(command);
            if (exitCode != 0)
                throw new TtsEngineException($"Error executing piper command '{command}', exit code {exitCode}");
        }

        private int ExecuteShell(String command, String arguments=null)
        {
            using (Process p = new Process())
            {
                p.StartInfo.UseShellExecute = true;
                if (Environment.OSVersion.Platform.ToString()
                    .StartsWith("win", StringComparison.InvariantCultureIgnoreCase))
                {
                    p.StartInfo.FileName = "cmd";
                    p.StartInfo.Arguments = "/C \"" + command + (String.IsNullOrEmpty(arguments)?"":" " + arguments) + "\"";
                }
                else
                {
                    p.StartInfo.FileName = "sh";
                    p.StartInfo.Arguments = "-c \"" + command + (String.IsNullOrEmpty(arguments)?"":" " + arguments) + "\"";
                }

                p.Start();
                if(Timeout<1)
                    p.WaitForExit();
                else
                    p.WaitForExit(Timeout);
                
                return p.ExitCode;
            }
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

        public static TtsEngine Instance(FileInfo piperCmd, DirectoryInfo basePiperLanguages)
        {
            if (_instance == null)
                _instance = new PiperTtsEngine(piperCmd, basePiperLanguages);

            return _instance;
        }
        
        public static TtsEngine Instance()
        {
            if (_instance == null)
                throw new TtsEngineException("Please, initialize instance before with parameters.");

            return _instance;
        }
    }
}