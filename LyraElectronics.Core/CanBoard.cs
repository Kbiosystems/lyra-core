using System;
using System.Collections.Generic;
using System.Text;
using LyraElectronics.Events;

namespace LyraElectronics
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class CanBoard
    {
        protected internal ICanController _controller;

        internal abstract Int32 Range { get; }

        /// <summary>
        ///     The CAN board address as a bit representation 
        ///     of its dip switch position
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

        public CanBoard(int address)
        {
            Address = address;
        }

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

        public void SendMessage(byte[] data)
        {
            _controller?.SendMessage(new CanMessage((Range | (Address << 4)), 8, data));
        }

        protected internal abstract void Parse(byte[] data);
    }
}
