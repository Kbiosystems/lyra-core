using LyraElectronics.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics
{
    /// <summary>
    ///     The CAN controller interface. Pass to <see cref="CanBoard"/> 
    ///     class for event based status parsing.
    /// </summary>
    public abstract class CanController
    {
        /// <summary>
        ///     Ocurrs when a CAN message is recieved.
        /// </summary>
        public event EventHandler<CanMessageRecievedEventArgs> CanMessageRecieved;

        /// <summary>
        ///     Opens the CAN channel. Baud rate defaults to 
        ///     250 in line with Lyra specs.
        /// </summary>
        /// <param name="baudRate">
        ///     An integer representing the baud rate for the 
        ///     CAN interface.
        /// </param>
        public abstract void OpenChannel(int baudRate = 250);

        /// <summary>
        ///     Closes the CAN channel.
        /// </summary>
        public abstract void CloseChannel();

        /// <summary>
        ///     Sends a <see cref="CanMessage"/> over the 
        ///     current CAN interface.
        /// </summary>
        /// <param name="message">
        ///     The <see cref="CanMessage"/> to send.
        /// </param>
        public abstract void SendMessage(CanMessage message);

        /// <summary>
        ///     Should be called when a can message is recieved 
        ///     by the controller. This method warps the logic 
        ///     required for the can board parsing to occur internally.
        /// </summary>
        /// <param name="message">
        ///     The <see cref="CanMessage"/> received.
        /// </param>
        protected virtual void OnCanMessageRecieved(CanMessage message)
        {
            CanMessageRecieved?.Invoke(this, new CanMessageRecievedEventArgs(message));
        }
    }
}
