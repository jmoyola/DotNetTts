using System;
using System.IO;

namespace DotNetTts.Helpers
{
    public class TempDirectory:IDisposable
    {
        private bool _disposed;
        private readonly DirectoryInfo _directory;

        private TempDirectory(DirectoryInfo file)
        {
            _directory = file ?? throw new ArgumentNullException(nameof(file));
        }
        
        public DirectoryInfo Directory=>_directory;
        
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
                _directory.Refresh();
                if(_directory.Exists)
                    _directory.Delete(true);
                _disposed = true;
            }
        }

        public override string ToString()
        {
            return _directory.FullName;
        }

        public override bool Equals(object obj)
        {
            return obj is TempDirectory && ((TempDirectory)obj)._directory.FullName==_directory.FullName;
        }

        public override int GetHashCode()
        {
            return _directory.FullName.GetHashCode();
        }

        public static implicit operator DirectoryInfo(TempDirectory v)
        {
            return v._directory;
        }
        
        public static implicit operator TempDirectory(DirectoryInfo v)
        {
            return new TempDirectory(v);
        }
        
        public static TempDirectory Create(String extension=null)
        {
            return new TempDirectory(new DirectoryInfo(Path.GetTempPath() + Guid.NewGuid() + extension));
        }
    }
}