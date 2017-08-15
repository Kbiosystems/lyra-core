using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    public class SequenceNumberChangedEventArgs : EventArgs
    {
        public int OldSequenceNumber { get; private set; }
        public int NewSequenceNumber { get; private set; }

        public SequenceNumberChangedEventArgs(int oldSequenceNumber, int newSequenceNumber)
        {
            OldSequenceNumber = oldSequenceNumber;
            NewSequenceNumber = newSequenceNumber;
        }
    }
}
