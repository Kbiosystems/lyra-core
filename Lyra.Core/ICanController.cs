using System;
using System.Collections.Generic;
using System.Text;

namespace Lyra
{
    public delegate void MessageRecievedEventHandler(object sender, CanMessage message);
    public enum ECanBps { Baud10kBps = 0, Baud20kBps = 1, Baud50kBps = 2, Baud100kBps = 3, Baud125kBps = 4, Baud250kBps = 5, Baud500kBps = 6, Baud800kBps = 7, Baud1MBps = 8 }
    public interface ICanController
    {
        event MessageRecievedEventHandler MessageRecieved;
        void Open();
        void Close();
        void OpenCanChannel();
        void CloseCanChannel();
        void SendMessage(CanMessage message);
    }
}
