using System;
using System.Collections.Generic;
using System.Text;

namespace Lyra
{
    public delegate void MessageRecievedEventHandler(object sender, CanMessage message);
    public interface ICanController
    {
        event MessageRecievedEventHandler MessageRecieved;
        void OpenChannel(int baudRate);
        void CloseChannel();
        void SendMessage(CanMessage message);
    }
}
