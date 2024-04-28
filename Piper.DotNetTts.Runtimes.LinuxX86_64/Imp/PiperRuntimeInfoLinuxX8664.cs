using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using DotNetTts.Helpers;
using PiperDotNetTts.Core;

namespace Piper.DotNetTts.Runtimes.Imp
{
    [ExcludeFromCodeCoverage]
    public class PiperRuntimeInfoLinuxX8664:PiperRuntimeInfo
    {
        public PiperRuntimeInfoLinuxX8664()
        {
            if(File.Exists(PiperCmdPath))
                Cmd.ExecuteShell($"chmod 775 '{PiperCmdPath}'");
        }
        public override string Platform => "linux";
        public override IEnumerable<string> Architectures =>new string[]{"x64", "amd64"};
        public override string PiperCmdPath =>AppContext.BaseDirectory + System.IO.Path.DirectorySeparatorChar
                                       + "runtimes" + System.IO.Path.DirectorySeparatorChar
                                       + "linux-x86_64" + System.IO.Path.DirectorySeparatorChar
                                       + "piper" + System.IO.Path.DirectorySeparatorChar
                                       + "piper";
    }
}