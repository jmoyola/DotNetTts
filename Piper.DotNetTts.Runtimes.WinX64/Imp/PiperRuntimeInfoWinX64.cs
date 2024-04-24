using System;
using System.Collections.Generic;
using PiperDotNetTts.Core;

namespace Piper.DotNetTts.Runtimes.Imp
{
    public class PiperRuntimeInfoWinX64:PiperRuntimeInfo
    {
        public override string Platform => "win";
        public override IEnumerable<string> Architectures =>new string[]{"x64", "amd64"};
        public override string PiperCmdPath => System.IO.Path.DirectorySeparatorChar
                                       + "runtimes" + System.IO.Path.DirectorySeparatorChar
                                       + "win-x64" + System.IO.Path.DirectorySeparatorChar
                                       + "piper" + System.IO.Path.DirectorySeparatorChar
                                       + "piper.exe";
    }
}