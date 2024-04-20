using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace DotNetTts.Core
{
    [Serializable]
    public class TtsEngineException:Exception
    {
        protected TtsEngineException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public TtsEngineException(string message) : base(message)
        {
        }

        public TtsEngineException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
