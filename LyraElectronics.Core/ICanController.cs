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
    public interface ICanController
    {
        /// <summary>
        ///     Ocurrs when a CAN message is recieved.
        /// </summary>
        event CanMessageRecievedEventHandler CanMessageRecieved;

        /// <summary>
        ///     Opens the CAN channel. Baud rate defaults to 
        ///     250 in line with Lyra specs.
        /// </summary>
        /// <param name="baudRate">
        ///     An integer representing the baud rate for the 
        ///     CAN interface.
        /// </param>
        void OpenChannel(int baudRate = 250);

        /// <summary>
        ///     Closes the CAN channel.
        /// </summary>
        void CloseChannel();

        /// <summary>
        ///     Sends a <see cref="CanMessage"/> over the 
        ///     current CAN interface.
        /// </summary>
        /// <param name="message">
        ///     The <see cref="CanMessage"/> to send.
        /// </param>
        void SendMessage(CanMessage message);
    }
}
