using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    /// <summary>
    ///     The arguments associate with the <see cref="CanBoard"/> output changed event.
    /// </summary>
    public class OutputChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     An integer value representing the output that has changed.
        /// </summary>
        public int Output { get; private set; }

        /// <summary>
        ///     The old value of the output.
        /// </summary>
        public bool OldValue { get; private set; }

        /// <summary>
        ///     The new value of the output.
        /// </summary>
        public bool NewValue { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="OutputChangedEventArgs"/> class.
        /// </summary>
        /// <param name="output">
        ///     An integer value representing the output that has changed.
        /// </param>
        /// <param name="oldValue">
        ///     The old value of the output.
        /// </param>
        /// <param name="newValue">
        ///     The new value of the output.
        /// </param>
        public OutputChangedEventArgs(int output, bool oldValue, bool newValue)
            : base()
        {
            Output = output;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
