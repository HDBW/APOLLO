// (c) Licensed to the HDBW under one or more agreements.
// The HDBW licenses this file to you under the MIT license.
using System.IO;

namespace De.HDBW.Apollo.Client.Helper
{
    public sealed class TempFile : IDisposable
    {
        private volatile bool _disposed;

        public TempFile()
            : this(Path.GetTempFileName())
        {
        }

        private TempFile(string fileName)
            : this(new FileInfo(fileName))
        {
        }

        private TempFile(FileInfo temporaryFile)
        {
            FileInfo = temporaryFile;
        }

        ~TempFile() => Dispose(false);

        public FileInfo FileInfo { get; private set; }

        public static implicit operator FileInfo(TempFile temporaryFile)
        {
            return temporaryFile.FileInfo;
        }

        public static implicit operator string(TempFile temporaryFile)
        {
            return temporaryFile.FileInfo.FullName;
        }

        public static explicit operator TempFile(FileInfo temporaryFile)
        {
            return new TempFile(temporaryFile);
        }

        public async Task SaveAsync(Stream stream)
        {
            using (var file = new FileStream(this, FileMode.Open))
            {
                await stream.CopyToAsync(file);
                await file.FlushAsync();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            try
            {
                FileInfo.Delete();
            }
            catch (Exception)
            {
                // Ignore
            }

            _disposed = true;
        }

        public void Move(string path, bool overwrite)
        {
            var tempFile = FileInfo.FullName;
            FileInfo.MoveTo(path, overwrite);
            FileInfo = new FileInfo(tempFile);
        }
    }
}
