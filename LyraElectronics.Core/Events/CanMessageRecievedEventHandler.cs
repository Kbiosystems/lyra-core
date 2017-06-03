using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    /// <summary>
    ///     Delegate for CAN message recieved inside the <see cref="ICanController"/> imlpementation.
    /// </summary>
    /// <param name="sender">
    ///     The object invoking the event.
    /// </param>
    /// <param name="message">
    ///     The <see cref="CanMessage"/> recieved.
    /// </param>
    /// <remarks>
    ///     The <see cref="CanBoard"/> uses this event to parse <see cref="CanMessage"/> results.
    /// </remarks>
    public delegate void CanMessageRecievedEventHandler(object sender, CanMessage message);
}
