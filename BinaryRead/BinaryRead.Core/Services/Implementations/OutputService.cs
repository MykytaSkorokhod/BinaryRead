using BinaryRead.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRead.Core.Services.Implementations
{
    public class OutputService : IOutputService
    {
        private const int byteLineWidth = 18;
        private const int binaryLineWidth = 27;

        private bool isProgressAsync;

        public double WritingProgess { get; set; }

        public OutputService()
        {
        }

        public OutputService(Func<string, Task> outCallback)
        {
            OutputActionAsync += outCallback;
        }

        public OutputService(Action<string> outCallback)
        {
            OutputAction += outCallback;
        }

        public event Func<string, Task> OutputActionAsync;
        public event Action<string> OutputAction;
        public event Action Finalize;

        public async Task ArrayToTableString(byte[] output, int outputLenth = 0)
        {
            if (output.Length == 0)
                throw new ArgumentException("File is empty");

            if (OutputActionAsync.GetInvocationList() == null || OutputActionAsync.GetInvocationList().Length == 0)
                isProgressAsync = false;
            else
                isProgressAsync = true;

            var writingLenth = outputLenth != 0 ? outputLenth : output.Length;

            for (int i = 0; i < writingLenth; i++)
            {
                if (i % byteLineWidth == 0 && i != 0)
                {
                    if (isProgressAsync)
                    {
                        await OutputActionAsync?.Invoke("|\n");
                        await OutputActionAsync?.Invoke($"{RowLine}\n");
                    }
                    else
                    {
                        OutputAction?.Invoke("|\n");
                        OutputAction?.Invoke($"{RowLine}\n");
                    }
                }
                if (isProgressAsync)
                    await OutputActionAsync?.Invoke(AlignCentre(output[i]));
                else
                    OutputAction?.Invoke(AlignCentre(output[i]));

                WritingProgess = (double) (i + 1) / writingLenth;
            }

            Finalize?.Invoke();
        }

        public async Task ArrayToTableString(bool[] output, int outputLenth = 0)
        {
            if (output.Length == 0)
                throw new ArgumentException("File is empty");

            var writingLenth = outputLenth != 0 ? outputLenth : output.Length;

            for (int i = 0; i < writingLenth; i++)
            {
                if (i % binaryLineWidth == 0 && i != 0)
                {
                    if (isProgressAsync)
                    {
                        await OutputActionAsync?.Invoke("|\n");
                        await OutputActionAsync?.Invoke($"{RowLine}\n");
                    }
                    else
                    {
                        OutputAction?.Invoke("|\n");
                        OutputAction?.Invoke($"{RowLine}\n");
                    }
                }
                if (isProgressAsync)
                    await OutputActionAsync?.Invoke($"| {Convert.ToInt16(output[i])} ");
                else
                    OutputAction?.Invoke($"| {Convert.ToInt16(output[i])} ");

                WritingProgess = (double)(i + 1) / writingLenth;
            }

            Finalize?.Invoke();
        }

        private string AlignCentre(byte value)
        {
            if (value < 10)
                return $"| {value}   ";
            if (value < 100)
                return $"| {value}  ";

            return $"| {value} ";
        }

        private string RowLine
        {
            get
            {
                string output = string.Empty;
                for (int i = 0; i < byteLineWidth * 6; i++)
                {
                    output += '-';
                }

                return output;
            }
        }
    }
}
