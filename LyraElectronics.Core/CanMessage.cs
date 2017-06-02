using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyraElectronics
{
    public class CanMessage
    {
        /// <summary>
        /// Gets the data.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        /// Gets the address.
        /// </summary>
        public int Address { get; private set; }

        /// <summary>
        /// Gets the length of the data.
        /// </summary>
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
