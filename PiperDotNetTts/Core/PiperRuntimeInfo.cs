using DotNetTts.Helpers;

namespace PiperDotNetTts.Core;

public abstract class PiperRuntimeInfo:RuntimeInfo, IPiperRuntimeInfo
{
    public abstract string PiperCmdPath { get; }
}