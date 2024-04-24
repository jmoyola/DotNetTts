using DotNetTts.Helpers;

namespace PiperDotNetTts.Core;

public interface IPiperRuntimeInfo:IRuntimeInfo
{
    string PiperCmdPath { get; }
}