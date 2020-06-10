using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRead.Core.Services.Interfaces
{
    public interface IOutputService
    {
        public double WritingProgess { get; set; }

        public event Func<string, Task> OutputActionAsync;

        public event Action<string> OutputAction;

        public event Action Finalize;

        public Task ArrayToTableString(byte[] output, int outputLenth = 0);

        public Task ArrayToTableString(bool[] output, int outputLenth = 0);
    }
}
