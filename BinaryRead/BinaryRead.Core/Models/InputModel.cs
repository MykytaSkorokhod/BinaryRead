using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryRead.Core.Models
{
    public class InputModel
    {
        public string SourceFilePath { get; set; }
        public string DestinationFilePath { get; set; }
        public OutputType OutputType { get; set; }
        public ShowType ShowType { get; set; }
    }
}
