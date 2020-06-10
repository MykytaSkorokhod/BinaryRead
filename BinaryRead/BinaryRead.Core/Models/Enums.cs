using System;
using System.Collections.Generic;
using System.Text;

namespace BinaryRead.Core.Models
{
    public enum OutputType
    {
        Unknown = 0,
        Byte = 1,
        Binary = 2
    }

    public enum ShowType
    {
        Unknown = 0,
        Display = 1,
        File = 2
    }

    public enum EncodingType
    {
        Default = 0,
        ASCII = 1,
        Unicode = 2,
        BigEndianUnicode = 3,
        UTF7 = 4,
        UTF8 = 5,
        UTF32 = 6
    }
}
