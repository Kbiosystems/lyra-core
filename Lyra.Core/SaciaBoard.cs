using Lyra.Events;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lyra
{
    public delegate void MotorStartedEventHandler(object sender);
    public delegate void MotorStoppedEventHandler(object sender);

    public class SaciaBoard : CanBoard
    {
        public int Position { get; private set; }
        public bool Input1 { get; private set; }
        public bool Input2 { get; private set; }
        public bool Input3 { get; private set; }
        public bool Output1 { get; private set; }
        public bool Output2 { get; private set; }
        public bool Output3 { get; private set; }
        public bool Disabled { get; private set; }
        public bool SpeedNotSet { get; private set; }
        public bool CurrentNotSet { get; private set; }
        public bool HomeNotSet { get; private set; }
        public bool OverTemperature { get; private set; }
        public bool Running { get; set; }

        public event MotorStartedEventHandler MotorStarted;
        public event MotorStoppedEventHandler MotorStopped;

        public SaciaBoard(int address, ICanController controller) 
            : base(address, controller)
        { }

        internal override void Parse(byte[] data)
        {
            Position = (((int)data[3]) << 32) | (((int)data[2]) << 16) | (((int)data[1]) << 8) | (int)data[0];

            Disabled = (data[5] & 0x80) > 0;
            SpeedNotSet = (data[5] & 0x40) > 0;
            CurrentNotSet = (data[5] & 0x20) > 0;
            HomeNotSet = (data[5] & 0x10) > 0;
            OverTemperature = (data[5] & 0x82) > 0;

            // set inputs and invoke input changed event where applicable
            if (Input1 != (data[4] & 0x01) > 0)
            {
                Input1 = (data[4] & 0x01) > 0;
                InputChanged?.Invoke(this, new InputChangedEventArgs(1, !Input1, Input1));
            }

            if (Input2 != (data[4] & 0x02) > 0)
            {
                Input2 = (data[4] & 0x02) > 0;
                InputChanged?.Invoke(this, new InputChangedEventArgs(2, !Input2, Input2));
            }

            if (Input3 != (data[4] & 0x04) > 0)
            {
                Input3 = (data[4] & 0x04) > 0;
                InputChanged?.Invoke(this, new InputChangedEventArgs(3, !Input3, Input3));
            }

            // set outputs and invoke output changed event where applicable
            if (Output1 != (data[6] & 0x01) > 0)
            {
                Output1 = (data[6] & 0x01) > 0;
                OutputChanged?.Invoke(this, new OutputChangedEventArgs(1, !Output1, Output1));
            }

            if (Output2 != (data[6] & 0x02) > 0)
            {
                Output2 = (data[6] & 0x02) > 0;
                OutputChanged?.Invoke(this, new OutputChangedEventArgs(2, !Output2, Output2));
            }

            if (Output3 != (data[6] & 0x04) > 0)
            {
                Output3 = (data[6] & 0x04) > 0;
                OutputChanged?.Invoke(this, new OutputChangedEventArgs(3, !Output3, Output3));
            }

            if (Running != (data[5] & 0x81) > 0)
            {
                Running = (data[5] & 0x81) > 0;
                if (Running)
                {
                    MotorStarted?.Invoke(this);
                }
                if (!Running)
                {
                    MotorStopped?.Invoke(this);
                }
            }
        }
    }
}
