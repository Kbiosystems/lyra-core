using Lyra.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace Lyra
{
    public delegate void MotorStartedEventHandler(object sender);
    public delegate void MotorStoppedEventHandler(object sender);

    public class SaciaBoard : CanBoard
    {
        internal override int Range { get { return 0x600; } }

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

        /// <summary>
        /// Enable this board.
        /// </summary>
        public void Enable()
        {
            byte[] data = new byte[8] { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        /// Set the board motor positions to zero.
        /// </summary>
        public void Zero()
        {
            byte[] data = new byte[8] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        public void Reset()
        {
            byte[] data = new byte[8] { 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        /// Set the motor speed.
        /// </summary>
        /// <param name="speed">The speed.</param>
        /// <param name="rampUp">The acceleration.</param>
        /// <param name="rampDown">The decceleration</param>
        public void SetSpeed(int speed, int rampUp, int rampDown)
        {
            byte[] data = new byte[8] { 0x02, 0x00, GetByte(speed, 0), GetByte(speed, 1), GetByte(rampUp, 0), GetByte(rampUp, 1), GetByte(rampDown, 0), GetByte(rampDown, 1), };
            Debug.WriteLine("Speed - " + string.Join(" ", data.Select(d => d.ToString("X2"))));
            SendMessage(data);
        }

        /// <summary>
        /// The the motor hold and run currents
        /// </summary>
        /// <param name="runCurrent">The run current.</param>
        /// <param name="holdCurrent">The hold current.</param>
        public void SetCurrent(int runCurrent, int holdCurrent)
        {
            byte[] data = new byte[8] { 0x03, 0x00, GetByte(holdCurrent, 0), GetByte(holdCurrent, 1), GetByte(runCurrent, 0), GetByte(runCurrent, 1), 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        /// Set specified output to on/off
        /// </summary>
        /// <param name="output">The output to set</param>
        /// <param name="value">The output value. If true, on, else off</param>
        public void SetOutput(int output, bool value)
        {
            bool out1 = output == 1 ? value : Output1;
            bool out2 = output == 2 ? value : Output2;
            bool out3 = output == 3 ? value : Output3;

            byte[] data = new byte[8] { 0x04, 0x00, (byte)(out1 ? 0x01 : 0x00), (byte)(out2 ? 0x01 : 0x00), (byte)(out3 ? 0x01 : 0x00), 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        public void Poll()
        {
            byte[] data = new byte[8] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        public void None()
        {
            byte[] data = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        /// Goto the specified motor position.
        /// </summary>
        /// <param name="position">The motor position to goto.</param>
        public virtual void Goto(int position)
        {
            Debug.WriteLine(GetByte(position, 0).ToString("X1") + GetByte(position, 1).ToString("X1") + GetByte(position, 2).ToString("X1") + GetByte(position, 3).ToString("X1"));
            byte[] data = new byte[8] { 0x01, 0x00, GetByte(position, 0), GetByte(position, 1), GetByte(position, 2), GetByte(position, 3), 0x0, 0x0 };
            SendMessage(data);
        }

        /// <summary>
        /// Run the motor for a specified number of steps.
        /// </summary>
        /// <param name="steps">The number of steps to run.</param>
        public void Run(int steps)  
        {
            Run(0, false, steps);          
        }

        /// <summary>
        /// Run the motor until an input is set to specified value or to a maximum number of steps.
        /// </summary>
        /// <param name="stopInput">The input to watch.</param>
        /// <param name="stopLogic">The value of the input to stop on.</param>
        /// <param name="maximumSteps">The maximum steps to run.</param>
        public void Run(int stopInput, bool stopLogic, int maximumSteps)
        {
            byte direction = 0x00;
            if (maximumSteps < 0)
            {
                direction = 0x01;
                maximumSteps *= -1;
            }
            Debug.WriteLine(GetByte(maximumSteps, 0).ToString("X2") + GetByte(maximumSteps, 1).ToString("X2") + GetByte(maximumSteps, 2).ToString("X2") + GetByte(maximumSteps, 3).ToString("X2"));

            int logic = stopLogic ? 0x01 : 0x00;

            byte[] data = new byte[8] { 0x08, direction, Convert.ToByte(stopInput), Convert.ToByte(logic), GetByte(maximumSteps, 0), GetByte(maximumSteps, 1), GetByte(maximumSteps, 2), GetByte(maximumSteps, 3), };
            SendMessage(data);
        }

        /// <summary>
        /// Stop the motor. Terminates movement.
        /// </summary>
        public void Stop()
        {
            byte[] data = new byte[8] { 0x09, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

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

        private byte GetByte(int integer, int index)
        {
            byte b = BitConverter.GetBytes(integer)[index];
            return b;
        }
    }
}
