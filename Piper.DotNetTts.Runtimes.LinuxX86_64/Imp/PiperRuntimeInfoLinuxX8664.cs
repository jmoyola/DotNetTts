using System;
using System.Collections.Generic;
using PiperDotNetTts.Core;

namespace Piper.DotNetTts.Runtimes.Imp
{
    public class PiperRuntimeInfoLinuxX8664:PiperRuntimeInfo
    {
        public override string Platform => "linux";
        public override IEnumerable<string> Architectures =>new string[]{"x64", "amd64"};
        public override string PiperCmdPath => System.IO.Path.DirectorySeparatorChar
                                       + "runtimes" + System.IO.Path.DirectorySeparatorChar
                                       + "linux-x86_64" + System.IO.Path.DirectorySeparatorChar
                                       + "piper" + System.IO.Path.DirectorySeparatorChar
                                       + "piper";
    }
}