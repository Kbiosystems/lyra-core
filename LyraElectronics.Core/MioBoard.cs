using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="LyraElectronics.CanBoard" />
    public class MioBoard : CanBoard
    {
        /// <summary>
        ///     An Int32 representation of hexadecimal addressing 
        ///     range of the board.
        /// </summary>
        internal override int Range { get { return 0x500; } }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MioBoard"/> class.
        /// </summary>
        /// <param name="sequenceNumber"></param>
        /// <param name="controller"></param>
        public MioBoard(int sequenceNumber, ICanController controller) 
            : base(sequenceNumber, controller)
        { }

        /// <summary>
        ///     Parse the specified CAN data
        /// </summary>
        /// <param name="data">
        ///     The data to parse.
        /// </param>
        protected internal override void Parse(byte[] data)
        {
            throw new NotImplementedException();
        }
    }
}
