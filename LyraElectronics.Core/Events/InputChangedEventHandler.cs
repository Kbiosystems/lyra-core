using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    /// <summary>
    ///     Handles <see cref="CanBoard"/> input changed events.
    /// </summary>
    /// <param name="sender">
    ///     The object invoking the event.
    /// </param>
    /// <param name="eventArgs">
    ///     The <see cref="InputChangedEventArgs"/> associated with this event.
    /// </param>
    public delegate void InputChangedEventHandler(object sender, InputChangedEventArgs eventArgs);
}
