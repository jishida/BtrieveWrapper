using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BtrieveWrapper
{
    public enum KeyType : byte
    {
        [KeyType("String", 0)]
        String = 0,
        [KeyType("Integer", 4, 1, 2, 4, 8)]
        Integer = 1,
        [KeyType("Float", 4, 4, 8)]
        Float = 2,
        [KeyType("Date", 4, 4)]
        Date = 3,
        [KeyType("Time", 4, 4)]
        Time = 4,
        [KeyType("Decimal", 16)]
        Decimal = 5,
        [KeyType("Money", 16)]
        Money = 6,
        [KeyType("Logical", 1, 1, 2)]
        Logical = 7,
        [KeyType("Numric", 0)]
        Numric = 8,
        [KeyType("BFloat", 4, 4, 8)]
        BFloat = 9,
        [KeyType("LString", 0)]
        LString = 10,
        [KeyType("ZString", 0)]
        ZString = 11,
        [KeyType("UnsignedBinary", 4, 1, 2, 4, 8)]
        UnsignedBinary = 14,
        [KeyType("Autoincrement", 4, 2, 4)]
        Autoincrement = 15,
        [KeyType("Bit", 1, 1)]
        Bit = 16,
        [KeyType("Numeric STS", 0)]
        Numericsts = 17,
        [KeyType("Numeric SA", 0)]
        Numericsa = 18,
        [KeyType("Currency", 8, 8)]
        Currency = 19,
        [KeyType("Timestamp", 8, 8)]
        Timestamp = 20,
        [KeyType("Clob", 8, 8)]
        Clob = 21,
        [KeyType("Blob", 8, 8)]
        Blob = 22,
        [KeyType("WString", 0)]
        WString = 25,
        [KeyType("WZString", 0)]
        WZString = 26,
        [KeyType("Guid", 16, 16)]
        Guid = 27,
        [KeyType("Numeric SLB", 0)]
        Numericslb = 28,
        [KeyType("Numeric SLS", 0)]
        Numericsls = 29,
        [KeyType("DateTime", 8, 8)]
        DateTime = 30,
        [KeyType("Numeric STB", 0)]
        Numericstb = 31,
        LegacyBinary = 254,
        LegacyString = 255
    }
}
