using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    /// <summary>
    ///     CAN message recieved event args.
    /// </summary>
    /// <remarks>
    ///     The <see cref="CanBoard"/> uses this event to parse <see cref="CanMessage"/> results.
    /// </remarks>
    public class CanMessageRecievedEventArgs : EventArgs
    {
        /// <summary>
        ///     The <see cref="CanMessage"/> recieved.
        /// </summary>
        public CanMessage Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CanMessageRecievedEventArgs"/> class.
        /// </summary>
        /// <param name="message">
        ///     The <see cref="CanMessage"/> recieved.
        /// </param>
        public CanMessageRecievedEventArgs(CanMessage message)
            : base()
        {
            Message = message;
        }
    }
}
