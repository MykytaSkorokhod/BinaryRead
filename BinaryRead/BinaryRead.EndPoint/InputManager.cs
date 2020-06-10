using BinaryRead.Core;
using BinaryRead.Core.Models;
using BinaryRead.Core.Services;
using BinaryRead.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace BinaryRead.EndPoint
{
    public class InputManager
    {
        private readonly IOutputService outputService;
        private readonly IFileService fileService;

        private InputModel inputModel;

        public InputManager(IOutputService _outputService, IFileService _fileService)
        {
            outputService = _outputService;
            fileService = _fileService;
        }

        public InputModel AskForArguments()
        {
            inputModel = new InputModel();

            bool isPathAnswered = false;
            while (!isPathAnswered)
            {
                ArgumentsPrint();

                try
                {
                    if (String.IsNullOrEmpty(inputModel.SourceFilePath))
                        AskForSourcePath();
                    if (!Enum.IsDefined(typeof(OutputType), inputModel.OutputType) || inputModel.OutputType == OutputType.Unknown)
                        AskForOutputType();
                    if (!Enum.IsDefined(typeof(ShowType), inputModel.ShowType) || inputModel.ShowType == ShowType.Unknown)
                    {
                        AskForShowType();
                        //if(inputModel.ShowType == ShowType.Display)
                        //    outputManager.OutputAction += Console.Write;
                        //if (inputModel.ShowType == ShowType.File)
                        //    outputManager.OutputActionAsync += fileService.StartWriteAsync()
                    }

                    if(inputModel.ShowType == ShowType.Display)
                    {
                        //if (inputModel.OutputType == OutputType.Byte)
                        //    outputService.ArrayToTableString(fileByteOutput);
                        //if (inputModel.OutputType == OutputType.Binary)
                        //    outputService.ArrayToTableString(fileBinaryOutput);

                        isPathAnswered = true;
                    }
                    if(inputModel.ShowType == ShowType.File)
                    {
                        AskForDestinationPath();

                        //if (inputModel.OutputType == OutputType.Byte)
                        //    FileService.WriteFileOutput(
                        //        destinationFilePath,
                        //        OutputManager.ArrayToTableString(fileByteOutput));
                        //if (inputModel.OutputType == OutputType.Binary)
                        //    FileService.WriteFileOutput(
                        //        destinationFilePath,
                        //        OutputManager.ArrayToTableString(fileBinaryOutput));

                        //Process.Start(destinationFilePath);

                        isPathAnswered = true;
                    }
                }
                catch (ArgumentException ex)
                {
                    Console.Clear();
                    Console.WriteLine($"--- {ex.Message}! ---");
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"FATAL ERROR {ex.Message}");
                    Console.WriteLine("Push key to close program");
                    Console.ReadKey();
                    Environment.Exit(0);
                }
            }

            return inputModel;
        }

        private void AskForSourcePath()
        {
            try
            {
                Console.Write("Enter path for file: ");
                inputModel.SourceFilePath = Console.ReadLine();
                fileService.ValidateSourcePath(inputModel.SourceFilePath);
            }
            catch(ArgumentException ex)
            {
                inputModel.SourceFilePath = null;
                throw ex;
            }
        }

        private void AskForOutputType()
        {
            Console.Write("Chose output type (");
            foreach (OutputType suit in (OutputType[])Enum.GetValues(typeof(OutputType)))
            {
                if ((int)suit != 0)
                    Console.Write($"{suit} - {(int)suit}; ");
            }
            Console.Write("): ");
            inputModel.OutputType = (OutputType)Convert.ToInt32(Console.ReadLine());
            if (!Enum.IsDefined(typeof(OutputType), inputModel.OutputType) || inputModel.OutputType == OutputType.Unknown)
            {
                inputModel.OutputType = default;
                throw new ArgumentException("Chose valid answer");
            }

            //if (inputModel.OutputType == OutputType.Binary)
            //    fileBinaryOutput = fileService.ReadAsBinary(inputModel.SourceFilePath);
            //if (inputModel.OutputType == OutputType.Byte)
            //    fileByteOutput = fileService.ReadAsBytes(inputModel.SourceFilePath);
        }

        private void AskForShowType()
        {
            Console.Write("Chose show type (");
            foreach (ShowType suit in (ShowType[])Enum.GetValues(typeof(ShowType)))
            {
                if ((int)suit != 0)
                    Console.Write($"{suit} - {(int)suit}; ");
            }
            Console.Write("): ");
            inputModel.ShowType = (ShowType)Convert.ToInt32(Console.ReadLine());
            if (!Enum.IsDefined(typeof(ShowType), inputModel.ShowType) || inputModel.ShowType == ShowType.Unknown)
            {
                inputModel.ShowType = default;
                throw new ArgumentException("Chose valid answer");
            }
        }

        private void AskForDestinationPath()
        {
            Console.Write("Enter path for output file (Leave line empty for save dir): ");
            inputModel.DestinationFilePath = Console.ReadLine();

            if (string.IsNullOrEmpty(inputModel.DestinationFilePath) || string.IsNullOrWhiteSpace(inputModel.DestinationFilePath))
            {
                var index = inputModel.SourceFilePath.LastIndexOf('\\');
                var dotIndex = inputModel.SourceFilePath.LastIndexOf('.');
                if (index > 0)
                {
                    inputModel.DestinationFilePath = inputModel.SourceFilePath.Substring(0, index);
                    var filename = inputModel.SourceFilePath
                        .Remove(0, inputModel.DestinationFilePath.Length + 1)
                        .Remove(dotIndex - inputModel.DestinationFilePath.Length - 1, inputModel.SourceFilePath.Length - dotIndex)
                        + $"{inputModel.OutputType}.txt";

                    inputModel.DestinationFilePath = Path.Combine(inputModel.DestinationFilePath, filename);
                }
            }
        }


        private void ArgumentsPrint()
        {
            if (!String.IsNullOrEmpty(inputModel.SourceFilePath))
                Console.WriteLine($"File to read: {inputModel.SourceFilePath}");

            if (Enum.IsDefined(typeof(OutputType), inputModel.OutputType) && inputModel.OutputType != OutputType.Unknown)
                Console.WriteLine($"Read type: {inputModel.OutputType}");

            if (!Enum.IsDefined(typeof(ShowType), inputModel.ShowType) && inputModel.ShowType != ShowType.Unknown)
                Console.WriteLine($"Show type: {inputModel.ShowType}");
        }
    }
}
