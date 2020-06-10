using BinaryRead.Core.Models;
using BinaryRead.Core.Services.Implementations;
using BinaryRead.Core.Services.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRead.EndPoint
{
    public class App
    {
        private readonly IFileService fileService;
        private readonly IOutputService outputService;
        private readonly InputManager inputManager;

        private IList fileOutput;

        private InputModel inputModel;

        public App(IFileService _fileService, IOutputService _outputService, InputManager _inputManager)
        {
            fileService = _fileService;
            outputService = _outputService;
            inputManager = _inputManager;
        }

        public async Task Start()
        {
            inputModel = inputManager.AskForArguments();

            await ReadData();

            await ShowData();

            Console.Write("Press any key to close program: ");
            Console.ReadKey();
        }

        private async Task ReadData()
        {
            if (inputModel.OutputType == OutputType.Binary)
                fileOutput = await fileService.ReadAsBinaryAsync(inputModel.SourceFilePath);
            if (inputModel.OutputType == OutputType.Byte)
                fileOutput = await fileService.ReadAsBytesAsync(inputModel.SourceFilePath);
        }

        private async Task ShowData()
        {
            if(inputModel.ShowType == ShowType.Display)
            {
                outputService.OutputAction += Console.Write;

                await StartOutput();

            }
            if(inputModel.ShowType == ShowType.File)
            {
                outputService.OutputActionAsync += fileService.StartWriteAsync(inputModel.DestinationFilePath);
                outputService.Finalize += fileService.CloseWrite;
                StartOutput();

                var progressTask = ShowProgressBar();
                progressTask.Wait();
            }
        }

        private async Task StartOutput()
        {
            if (inputModel.OutputType == OutputType.Binary)
                await outputService.ArrayToTableString(fileOutput.Cast<bool>().ToArray());
            if (inputModel.OutputType == OutputType.Byte)
                await outputService.ArrayToTableString(fileOutput.Cast<byte>().ToArray());
        }

        private async Task ShowProgressBar()
        {
            using (var progress = new ProgressBar())
            {
                while (fileService.IsWritingProgressWork)
                {
                    progress.Report(outputService.WritingProgess);
                    await Task.Delay(20);
                }
            }
        }
    }
}
