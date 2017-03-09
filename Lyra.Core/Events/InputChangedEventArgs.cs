using System;
using System.Collections.Generic;
using System.Text;

namespace Lyra.Events
{
    public class InputChangedEventArgs : EventArgs
    {
        public int Input { get; private set; }
        public bool OldValue { get; private set; }
        public bool NewValue { get; private set; }
        public InputChangedEventArgs(int input, bool oldValue, bool newValue)
            : base()
        {
            Input = input;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
