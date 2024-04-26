using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using DotNetTts.Core;
using DotNetTts.Helpers;

namespace FfPlay.DotNetTts.Runtimes.Imp;

public class FfPlaySoundPlayer:SoundPlayer
{
    private static SoundPlayer _instance;
    
    private readonly FileInfo _ffplayPath;
    public FfPlaySoundPlayer(FileInfo ffplayPath)
    {
        _ffplayPath = ffplayPath ?? throw new ArgumentNullException(nameof(ffplayPath));

        if (!ffplayPath.Exists)
            throw new SoundPlayerException($"Ffplay path {ffplayPath.FullName} don't exists.");
    }

    public override void Play(string path)
    {
        Process p = new Process();
        p.StartInfo.UseShellExecute = false;
        p.StartInfo.FileName = _ffplayPath.FullName;
        p.StartInfo.Arguments = $" -v 0 -autoexit -nodisp \"{path}\"";
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.RedirectStandardOutput = true;
        p.Start();
        p.WaitForExit();
        int exitCode = p.ExitCode;
        if(exitCode!=0)
            throw new SoundPlayerException($"Ffplay error playing {path}: {p.StandardError.ReadToEnd()} {p.StandardOutput.ReadToEnd()}");
    }

    public static SoundPlayer Instance()
    {
        if(_instance==null)
        {
            string path = PathResolver.ProgramDirectory.First() + Path.DirectorySeparatorChar + "runtimes";
            
            switch (Environment.OSVersion.Platform.ToString().Substring(0,3))
            {
                case "Win":
                    path += Path.DirectorySeparatorChar + "win-x64" + Path.DirectorySeparatorChar + "ffplay.exe";
                    break;
                case "Uni":
                    path += Path.DirectorySeparatorChar + "linux-x86_x64" + Path.DirectorySeparatorChar + "ffplay";
                    break;
                case "Mac":
                    path += Path.DirectorySeparatorChar + "macos_x64" + Path.DirectorySeparatorChar + "ffplay";
                    break;
                default:
                    throw new SoundPlayerException($"Platform {Environment.OSVersion.Platform} is not supported.");
            }

            _instance = new FfPlaySoundPlayer(new FileInfo(path));
        }

        return _instance;
    }
}