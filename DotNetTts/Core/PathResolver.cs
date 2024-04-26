using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace DotNetTts.Core;

public static class PathResolver
{
    public static IEnumerable<string> ProgramDirectory=> new string[]{AppContext.BaseDirectory};

    public static IEnumerable<string> ProgramDeepDirectories => GetDirectories(AppContext.BaseDirectory, Int32.MaxValue);

    public static IEnumerable<string> GetDirectories(string path, int deepLevel = Int32.MaxValue)
    {
        List<string> ret = new List<string>();
        GetDirectoriesRecursive(path, ret, deepLevel);
        return ret;
    }
    
    private static void GetDirectoriesRecursive(string path, List<string> ret, int deepLevel = Int32.MaxValue)
    {
        if (deepLevel < 1)
            return;

        ret.Add(path);
        deepLevel--;

        foreach (string subPath in Directory.GetDirectories(path))
            GetDirectoriesRecursive(subPath, ret, deepLevel);
    }

    public static IEnumerable<FileInfo> FindProgram(string programName, IEnumerable<string> additionalPaths = null)
    {
        string ext = Path.GetExtension(programName);
        bool isWindows = Environment.OSVersion.Platform.ToString().StartsWith("win", StringComparison.InvariantCultureIgnoreCase);
        
        programName=programName + (isWindows && String.IsNullOrEmpty(ext)?".exe":"");
        
        return FindFile(programName, additionalPaths);
    }

    public static IEnumerable<FileInfo> FindFile(string fileName, IEnumerable<string> additionalPaths = null)
    {
        bool ignoreCase = Environment.OSVersion.Platform.ToString().StartsWith("Win");
        return FindRegex("^" + fileName + "$", ignoreCase ? RegexOptions.IgnoreCase : RegexOptions.None, additionalPaths);
    }
    
    public static IEnumerable<FileInfo> FindRegex(string fileNamePattern, RegexOptions regexOptions= RegexOptions.None,  IEnumerable<string> additionalPaths=null)
    {
        if (fileNamePattern == null)
            throw new ArgumentNullException(nameof(fileNamePattern));
        
        List<FileInfo> ret = new List<FileInfo>();

        String pathVar = Environment.GetEnvironmentVariable("PATH");
        
        if(pathVar==null)
            return ret;

        List<string> pathList = pathVar.Split(Path.PathSeparator).ToList();
        if(additionalPaths!=null)
            pathList.AddRange(additionalPaths);

        Regex r = new Regex(fileNamePattern, regexOptions);
        
        foreach (string pathFolder in pathList)
        {
            ret.AddRange(new DirectoryInfo(pathFolder).GetFiles()
                .Where(v=>r.IsMatch(v.Name))
            );
        }
        
        return ret;
    }
}