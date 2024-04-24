using System.Collections.Generic;

namespace DotNetTts.Helpers;

public interface IRuntimeInfo
{
    string Platform { get; }
    IEnumerable<string> Architectures { get; }
    
    bool IsCompatible { get; }
}