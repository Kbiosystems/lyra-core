using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    /// <summary>
    ///     The arguments associate with the <see cref="CanBoard"/> input changed event.
    /// </summary>
    public class InputChangedEventArgs : EventArgs
    {
        /// <summary>
        ///     An integer value representing the input that has changed.
        /// </summary>
        public int Input { get; private set; }

        /// <summary>
        ///     The old value of the input.
        /// </summary>
        public bool OldValue { get; private set; }

        /// <summary>
        ///     The new value of the input.
        /// </summary>
        public bool NewValue { get; private set; }

        /// <summary>
        ///     Initializes a new instance of the <see cref="InputChangedEventArgs"/> class.
        /// </summary>
        /// <param name="input">
        ///     An integer value representing the input that has changed.
        /// </param>
        /// <param name="oldValue">
        ///     The old value of the input.
        /// </param>
        /// <param name="newValue">
        ///     The new value of the input.
        /// </param>
        public InputChangedEventArgs(int input, bool oldValue, bool newValue)
            : base()
        {
            Input = input;
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
