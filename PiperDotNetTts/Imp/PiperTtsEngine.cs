using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using DotNetTts.Helpers;
using Newtonsoft.Json;

namespace PiperDotNetTts.Imp
{
    public class PiperTtsEngine:TtsEngine
    {
        private static TtsEngine _instance;
        private List<PiperVoice> _piperVoices; 
        private IEnumerable<TtsVoiceInfo> _voices;
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
        
        public override FileInfo Speech(String text, TtsVoiceInfo voiceInfo, TtsProperties ttsProperties = null)
        {
            TempFile tempFile = TempFile.Create(".wav");
            Speech(text, voiceInfo, tempFile, ttsProperties);
            return tempFile;
        }
        
        public override void Speech(string text, TtsVoiceInfo voiceInfo, FileInfo outputWavFile, TtsProperties ttsProperties = null)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));
            
            if (voiceInfo == null)
                throw new ArgumentNullException(nameof(voiceInfo));
            
            if (outputWavFile == null)
                throw new ArgumentNullException(nameof(outputWavFile));
            
            TtsVoiceInfo vi = _voices.FirstOrDefault(v => v.Equals(voiceInfo));
            
            if (vi == null)
                throw new TtsEngineException($"Voice '{voiceInfo}' is not available.");

            ExecutePiper(text, new FileInfo(vi.Path), outputWavFile);
        }

        public override IEnumerable<TtsVoiceInfo> Voices
        {
            get
            {
                if(_voices==null)
                    _voices= _piperVoices.Select(v => new TtsVoiceInfo(v.LanguageFile.FullName, v.Language, v.Info.Dataset, v.Info.Audio.Quality));

                return _voices;
            }
        }

        private void ExecutePiper(String text, FileInfo modelPath, FileInfo outputPath)
        {
            int exitCode;
            string command;
            if (Environment.OSVersion.Platform.ToString()
                .StartsWith("win", StringComparison.InvariantCultureIgnoreCase))
            {
                command="echo \"" + text + "\""
                                     + " | \"" + _piperCmd.FullName + "\""
                                     + " --model \"" + modelPath + "\""
                                     + " --output-file \"" + outputPath + "\"";
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
                if (Environment.OSVersion.Platform.ToString()
                    .StartsWith("win", StringComparison.InvariantCultureIgnoreCase))
                {
                    p.StartInfo.UseShellExecute = false;
                    p.StartInfo.FileName = "CMD";
                    p.StartInfo.Arguments = "/C " + command + (String.IsNullOrEmpty(arguments)?"":" " + arguments);
                }
                else
                {
                    p.StartInfo.UseShellExecute = true;
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
            _piperVoices = new List<PiperVoice>();
            FindVoices(_basePiperLanguages, _piperVoices);
            
            JsonSerializer jss = JsonSerializer.Create();
            
            foreach (PiperVoice pv in _piperVoices)
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
            voices.AddRange(folder.GetFiles("*.onnx")
                .Where(v=>v.Directory.GetFiles(v.Name+".json").Any())
                .Select(v=>new PiperVoice(){LanguageFile=v}));
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