using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LyraElectronics
{
    /// <summary>
    ///     The CAN message class representing the CAN 
    ///     message revieved or ready to send.
    /// </summary>
    public class CanMessage
    {
        /// <summary>
        ///     The CAN message data.
        /// </summary>
        public byte[] Data { get; private set; }

        /// <summary>
        ///     The CAN message target address (this is not 
        ///     the board sequence number specified by the 
        ///     <see cref="CanBoard"/> class).
        /// </summary>
        /// <remarks>
        ///     Please note the message address will be 
        ///     different depending on the direction the 
        ///     message is going
        /// </remarks>
        public int Address { get; private set; }

        /// <summary>
        ///     The length of the CAN message data byte[] array
        /// </summary>
        public int DataLength { get; private set; }


        /// <summary>
        ///     Initializes a new instance of the <see cref="CanMessage"/> class.
        /// </summary>
        /// <param name="address">
        ///     The CAN message target address (this is not 
        ///     the board sequence number specified by the 
        ///     <see cref="CanBoard"/> class).
        /// </param>
        /// <param name="dataLength">
        ///     The length of the CAN message data byte[] array
        /// </param>
        /// <param name="data">
        ///     The CAN message data.
        /// </param>
        public CanMessage(int address, int dataLength, byte[] data)
        {
            Data = data;
            DataLength = dataLength;
            Address = address;
        }


        /// <summary>
        ///     Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        ///     A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Convert.ToString(Address, 16) + " " + Convert.ToString(DataLength, 16) + " " + string.Join(" ", Data.Select(d => Convert.ToString(d, 16)));
        }

        /// <summary>
        ///     Converts the btye[] representation of a 
        ///     can message to a <see cref="CanMessage"/> 
        ///     object equivalent.
        /// </summary>
        /// <param name="bytes">
        ///     The byte array to parse.
        /// </param>
        /// <returns>
        ///     The parsed <see cref="CanMessage" /> object.
        /// </returns>
        public static CanMessage Parse(byte[] bytes)
        {
            int id = int.Parse(Encoding.ASCII.GetString(bytes.SubArray(0, 3)), System.Globalization.NumberStyles.HexNumber);
            int length = int.Parse(Encoding.ASCII.GetString(bytes.SubArray(3, 1)), System.Globalization.NumberStyles.HexNumber);
            List<byte> data = new List<byte>();
            for (int j = 4; j < 4 + (length * 2); j += 2)
            {
                data.Add(byte.Parse(Encoding.ASCII.GetString(bytes.SubArray(j, 2)), System.Globalization.NumberStyles.HexNumber));
            }

            return new CanMessage(id, length, data.ToArray());
        }
    }
}
