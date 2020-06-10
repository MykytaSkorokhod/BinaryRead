using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRead.Core.Services.Interfaces
{
    public interface IFileService
    {
        public bool IsWritingProgressWork { get; }

        public Task<byte[]> ReadAsBytesAsync(string path);

        public Task<bool[]> ReadAsBinaryAsync(string path);

        public Action<string> StartWrite(string path);

        public Func<string, Task> StartWriteAsync(string path);

        public void CloseWrite();

        public void ValidateSourcePath(string path);
    }
}
