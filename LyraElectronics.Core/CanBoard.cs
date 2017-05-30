using System;
using System.Collections.Generic;
using System.Text;
using LyraElectronics.Events;

namespace LyraElectronics
{
    public abstract class CanBoard
    {
        protected internal ICanController _controller;

        internal abstract Int32 Range { get; }

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
            controller.CanMessageRecieved += (s, m) =>
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

        internal abstract void Parse(byte[] data);
    }
}
