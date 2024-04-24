using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DotNetTts.Helpers;

public static class Plugins
{
    private static void AvailableDeep<T>(DirectoryInfo directory, List<Type> output, int deep=1)
    {
        foreach (FileInfo fi in directory.GetFiles("*.dll"))
        {
            try
            {
                Assembly ass=Assembly.LoadFile(fi.FullName);
                output.AddRange(Available<T>(ass));
            }
            catch
            {
                // do nothing for no assembly dlls
            }
        }

        deep--;
        
        if (deep > 0)
        {
            foreach (DirectoryInfo di in directory.GetDirectories())
                AvailableDeep<T>(di, output, deep);
        }

    }
    
    public static IEnumerable<Type> Available<T>(Assembly assembly)
    {
        Type baseType = typeof(T);
        return assembly.GetTypes().Where(t => baseType.IsAssignableFrom(t)
                                              && !t.IsInterface && !t.IsAbstract);
    }
    
    public static IEnumerable<Type> Available<T>(DirectoryInfo directory, int deep=1)
    {
        List<Type> ret=new List<Type>();

        AvailableDeep<T>(directory, ret, deep);

        return ret;
    }

    public static IEnumerable<Type> Available<T>()
    {
        return Available<T>(new DirectoryInfo(AppContext.BaseDirectory), Int32.MaxValue);
    }

    public static IEnumerable<T> AvailableInstances<T>(params object[] parameters)
    {
        return Available<T>().Select(t=>(T)Activator.CreateInstance(t, parameters));
    }
    
    public static IEnumerable<T> AvailableInstances<T>(Assembly assembly, params object[] parameters)
    {
        return Available<T>(assembly).Select(t=>(T)Activator.CreateInstance(t, parameters));
    }
    
    public static IEnumerable<T> AvailableInstances<T>(DirectoryInfo directory, int deep=1, params object[] parameters)
    {
        return Available<T>(directory, deep).Select(t=>(T)Activator.CreateInstance(t, parameters));
    }
}