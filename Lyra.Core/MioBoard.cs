using System;
using System.Collections.Generic;
using System.Text;

namespace Lyra
{
    public class MioBoard : CanBoard
    {
        internal override int Range { get { return 0x500; } }

        public MioBoard(int address, ICanController controller) 
            : base(address, controller)
        { }

        internal override void Parse(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
