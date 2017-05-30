using LyraElectronics.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace LyraElectronics
{
    public interface ICanController
    {
        event CanMessageRecievedEventHandler MessageRecieved;
        void OpenChannel(int baudRate = 250);
        void CloseChannel();
        void SendMessage(CanMessage message);
    }
}
