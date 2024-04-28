using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace DotNetTts.Helpers;

[ExcludeFromCodeCoverage]
public abstract class RuntimeInfo:IRuntimeInfo
{
    public abstract string Platform { get; }
    public abstract IEnumerable<string> Architectures { get; }

    public bool IsCompatible
    {
        get
        {
            return this.Platform.Equals(OsPlatform, StringComparison.InvariantCultureIgnoreCase)
                   && this.Architectures.Any(a =>
                       a.Equals(OsArchitecture, StringComparison.InvariantCultureIgnoreCase));
        }
    }
    
    public static string OsPlatform{
        get {
        if (Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.InvariantCultureIgnoreCase))
            return "win";

        if (Environment.OSVersion.Platform == PlatformID.Unix)
            return "linux";

        if (Environment.OSVersion.Platform == PlatformID.MacOSX)
            return "macos";

        return "unknow";
        }
    }
    
    public static string OsArchitecture
    {
        get
        {
            if (Environment.Is64BitOperatingSystem)
                return "x64";

            return "x32";
        }
    }
}