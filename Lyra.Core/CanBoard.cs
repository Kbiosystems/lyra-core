using System;
using System.Collections.Generic;
using System.Text;
using Lyra.Events;

namespace Lyra
{
    public abstract class CanBoard
    {
        protected internal ICanController _controller;

        public int Address { get; private set; }

        public InputChangedEventHandler InputChanged;
        public OutputChangedEventHandler OutputChanged;

        public CanBoard(int address)
        {
            Address = address;
        }

        public CanBoard(int address, ICanController controller)
            : this (address)
        {
            controller.MessageRecieved += (s, m) =>
            {
                if (m.Address == ((0x600 | (Address << 4)) | 8))
                {
                    Parse(m.Data);
                }
            };
        }

        public virtual void Enable()
        {
            byte[] data = new byte[8] { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        public virtual void Zero()
        {
            byte[] data = new byte[8] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        public virtual void Reset()
        {
            byte[] data = new byte[8] { 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        public void SendMessage(byte[] data)
        {
            _controller?.SendMessage(new CanMessage((0x600 | (Address << 4)), 8, data));
        }

        internal abstract void Parse(byte[] data);
    }
}
