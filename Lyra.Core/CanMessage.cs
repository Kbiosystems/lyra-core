using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lyra
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
            return "ID: " + Convert.ToString(Address, 16) + " Data: " + string.Join(" ", Data.Select(d => Convert.ToString(d, 16)));
        }

    }
}
