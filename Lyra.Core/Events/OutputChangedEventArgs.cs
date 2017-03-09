using System;
using System.Collections.Generic;
using System.Text;

namespace Lyra.Events
{
    public class OutputChangedEventArgs : EventArgs
    {
        public int Output { get; private set; }
        public bool OldValue { get; private set; }
        public bool NewValue { get; private set; }
        public OutputChangedEventArgs(int output, bool oldValue, bool newValue)
            : base()
        {
            Output = output;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
