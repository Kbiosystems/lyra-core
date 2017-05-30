using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics
{
    public class MioBoard : CanBoard
    {
        internal override int Range { get { return 0x500; } }

        public MioBoard(int address, ICanController controller) 
            : base(address, controller)
        { }

        protected internal override void Parse(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
