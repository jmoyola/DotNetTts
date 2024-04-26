using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace DotNetTts.Helpers;

public static class Cmd
{
    [Serializable]
    public class CmdException:Exception
    {
        protected CmdException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public CmdException(string message) : base(message) { }
        public CmdException(string message, Exception innerException) : base(message, innerException) { }
    }

    public static void Execute(string path, string arguments = "", int timeout = int.MaxValue, bool useExitCode = false)
    {
        Process p=null;
        try
        {
            p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.Arguments = arguments;
            p.StartInfo.FileName = path;
            p.Start();

            if (timeout > 0)
                p.WaitForExit(timeout);
            else
                p.WaitForExit();

            int exitCode = p.ExitCode;

            if (useExitCode && exitCode != 0)
                throw new CmdException($"Error executing {path} {arguments}: {p.StandardError.ReadToEnd()}");
        }
        finally
        {
            p?.Dispose();
        }
    }
    
    public static void ExecuteShell(String path, String arguments=null, int timeout = int.MaxValue, bool useExitCode = false)
    {
        Process p = null;
        try
        {
            p = new Process();
                
            if (Environment.OSVersion.Platform.ToString()
                .StartsWith("win", StringComparison.InvariantCultureIgnoreCase))
            {
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.UseShellExecute = false;
                p.StartInfo.RedirectStandardError = true;
                p.StartInfo.FileName = "CMD";
                p.StartInfo.Arguments = "/C " + path + (String.IsNullOrEmpty(arguments) ? "" : " " + arguments);
            }
            else
            {
                p.StartInfo.UseShellExecute = true;
                p.StartInfo.FileName = "sh";
                p.StartInfo.Arguments =
                    "-c \"" + path + (String.IsNullOrEmpty(arguments) ? "" : " " + arguments) + "\"";
            }

            p.Start();
            
            if (timeout > 0)
                p.WaitForExit();
            else
                p.WaitForExit(timeout);

            int exitCode = p.ExitCode;

            if (useExitCode && exitCode != 0)
                throw new CmdException($"Error executing {path} {arguments}: {p.StandardError.ReadToEnd()}");
        }
        finally
        {
            p?.Dispose();
        }
    }
}