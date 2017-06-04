using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics.Events
{
    /// <summary>
    ///     Delegate for <see cref="CanBoard"/> motor stopped event.
    /// </summary>
    /// <param name="sender">
    ///     The object invoking the event.
    /// </param>
    public delegate void MotorStoppedEventHandler(object sender);
}
