using System;
using System.IO;

namespace DotNetTts.Helpers
{
    public class TempFile:IDisposable
    {
        private bool _disposed;
        private readonly FileInfo _file;

        private TempFile(FileInfo file)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
        }
        
        public FileInfo File=>_file;
        
        public void Dispose()
        {
            GC.SuppressFinalize(this);
            Dispose(true);
        }
        
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;
            
            if(disposing)
            {
                _file.Refresh();
                if(_file.Exists)
                    _file.Delete();
                _disposed = true;
            }
        }

        public override string ToString()
        {
            return _file.FullName;
        }

        public override bool Equals(object obj)
        {
            return obj is TempFile && ((TempFile)obj)._file.FullName==_file.FullName;
        }

        public override int GetHashCode()
        {
            return _file.FullName.GetHashCode();
        }

        public static implicit operator FileInfo(TempFile v)
        {
            return v._file;
        }
        
        public static implicit operator TempFile(FileInfo v)
        {
            return new TempFile(v);
        }
        
        public static TempFile Create()
        {
            return new TempFile(new FileInfo(Path.GetTempPath() + Guid.NewGuid()));
        }
    }
}