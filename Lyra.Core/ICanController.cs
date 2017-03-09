using System;
using System.Collections.Generic;
using System.Text;

namespace Lyra
{
    public delegate void MessageRecievedEventHandler(object sender, CanMessage message);

    public interface ICanController
    {
        event MessageRecievedEventHandler MessageRecieved;

        void SendMessage(CanMessage message);
    }
}
