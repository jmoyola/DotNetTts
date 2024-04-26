﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace DotNetTts.Core;

[ExcludeFromCodeCoverage]
[Serializable]
public class SoundPlayerException : Exception
{
    protected SoundPlayerException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public SoundPlayerException(string message) : base(message)
    {
    }

    public SoundPlayerException(string message, Exception innerException) : base(message, innerException)
    {
    }
}

public abstract class SoundPlayer
{
    public abstract void Play(string path);
}