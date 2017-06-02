using System;
using System.Collections.Generic;
using System.Text;
using LyraElectronics.Events;

namespace LyraElectronics
{
    /// <summary>
    ///     The CanBoard base class.
    /// </summary>
    public abstract class CanBoard
    {
        /// <summary>
        ///     The <see cref="ICanController"/> implementation associated with this board.
        /// </summary>
        protected internal ICanController _controller;

        /// <summary>
        ///     An Int32 representation of hexadecimal addressing range of the board.
        /// </summary>
        internal abstract Int32 Range { get; }

        /// <summary>
        ///     The CAN board address as a integer representation 
        ///     of its binary dip switch position
        /// </summary>
        /// <remarks>
        ///     Different board types use seperate addressing ranges. 
        ///     As such its possible to have two different board types 
        ///     with the same dip switch setting. In other words, a 
        ///     single CAN line may have both a single Sacia and single 
        ///     Mio board with the same numeric address. Regarding dip 
        ///     switches see the individual Lyra board documentation.
        /// </remarks>
        public int Address { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CanBoard"/> class.
        /// </summary>
        /// <param name="address">
        ///     The CAN board address as a integer representation 
        ///     of its binary dip switch position
        /// </param>
        internal CanBoard(int address)
        {
            Address = address;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="CanBoard"/> class.
        /// </summary>
        /// <param name="address">
        ///     The CAN board address as a integer representation 
        ///     of its binary dip switch position
        /// </param>
        /// <param name="controller">
        ///     The <see cref="ICanController"/> associated with this board. 
        /// </param>
        public CanBoard(int address, ICanController controller)
            : this (address)
        {
            _controller = controller;

            _controller.CanMessageRecieved += (s, m) =>
            {
                if (m.Address == ((Range | (Address << 4)) | 8))
                {
                    Parse(m.Data);
                }
            };
        }

        /// <summary>
        ///     Sends the CAN byte data to the associate board.
        /// </summary>
        /// <param name="data">
        ///     The CAN data to send.
        /// </param>
        public void SendMessage(byte[] data)
        {
            _controller?.SendMessage(new CanMessage((Range | (Address << 4)), 8, data));
        }

        /// <summary>
        ///     Parses the specified CAN data
        /// </summary>
        /// <param name="data">
        ///     The data to parse.
        /// </param>
        protected internal abstract void Parse(byte[] data);
    }
}
