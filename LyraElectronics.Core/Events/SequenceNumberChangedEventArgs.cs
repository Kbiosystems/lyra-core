using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    /// <summary>
    ///     The arguments associated with the <see cref="CanBoard"/> sequence number changed event.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class SequenceNumberChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     The old sequence number.
        /// </summary>
        public int OldSequenceNumber { get; private set; }

        /// <summary>
        ///     The new sequence number.
        /// </summary>
        public int NewSequenceNumber { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SequenceNumberChangedEventArgs"/> class.
        /// </summary>
        /// <param name="oldSequenceNumber">
        ///     The old sequence number.
        /// </param>
        /// <param name="newSequenceNumber">
        ///     The new sequence number.
        /// </param>
        public SequenceNumberChangedEventArgs(int oldSequenceNumber, int newSequenceNumber)
        {
            OldSequenceNumber = oldSequenceNumber;
            NewSequenceNumber = newSequenceNumber;
        }
    }
}
