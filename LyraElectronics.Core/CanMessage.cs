using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyraElectronics
{
    public class CanMessage
    {
        public byte[] Data { get; private set; }
        public int Address { get; private set; }
        public int DataLength { get; private set; }

        public CanMessage(int address, int dataLength, byte[] data)
        {
            Data = data;
            DataLength = dataLength;
            Address = address;
        }

        public override string ToString()
        {
            return Convert.ToString(Address, 16) + " " + Convert.ToString(DataLength, 16) + " " + string.Join(" ", Data.Select(d => Convert.ToString(d, 16)));
        }

    }
}
