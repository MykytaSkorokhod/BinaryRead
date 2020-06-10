using BinaryRead.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRead.Core.Services.Implementations
{
    public class FileService : IFileService
    {
        private StreamWriter streamWriter;

        public bool IsWritingProgressWork => streamWriter != null;

        public async Task<byte[]> ReadAsBytesAsync(string path)
        {
            ValidateSourcePath(path);

            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] array = new byte[fstream.Length];
                await fstream.ReadAsync(array, 0, array.Length);

                return array;
            }
        }

        public async Task<bool[]> ReadAsBinaryAsync(string path)
        {
            ValidateSourcePath(path);

            using (FileStream fstream = File.OpenRead(path))
            {
                byte[] array = new byte[fstream.Length];
                await fstream.ReadAsync(array, 0, array.Length);

                List<bool> result = new List<bool>();

                foreach (var item in array)
                {
                    result.AddRange(ConvertByteToBoolArray(item));
                }

                return result.ToArray();
            }
        }

        public Action<string> StartWrite(string path)
        {
            streamWriter = new StreamWriter(path, false, Encoding.Default);

            return WriteCallBack;
        }

        public Func<string, Task> StartWriteAsync (string path)
        {
            streamWriter = new StreamWriter(path, false, Encoding.Default);

            return WriteCallBackAsync;
        }

        public void CloseWrite()
        {
            streamWriter.Dispose();
            streamWriter = null;
        }

        public void ValidateSourcePath(string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentException("Path is empty");
            if (!File.Exists(path))
                throw new ArgumentException("File not exist or program not have permission with this file");
        }

        private void WriteCallBack(string input)
        {
            if (streamWriter == null)
                throw new Exception("Stream not open for write");

            streamWriter.Write(input);
        }

        private async Task WriteCallBackAsync(string input)
        {
            if (streamWriter == null)
                throw new Exception("Stream not open for write");

            await streamWriter.WriteAsync(input);
        }

        private bool[] ConvertByteToBoolArray(byte b)
        {
            bool[] result = new bool[8];

            for (int i = 0; i < 8; i++)
                result[i] = (b & (1 << i)) == 0 ? false : true;

            Array.Reverse(result);

            return result;
        }
    }
}