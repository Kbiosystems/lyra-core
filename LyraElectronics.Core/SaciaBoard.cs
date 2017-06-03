using LyraElectronics.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LyraElectronics
{
    /// <summary>
    ///     
    /// </summary>
    /// <seealso cref="LyraElectronics.CanBoard" />
    public class SaciaBoard : CanBoard
    {
        /// <summary>
        ///     The hex addressing range of the board.
        /// </summary>
        internal override int Range { get { return 0x600; } }

        /// <summary>
        ///     The current motor position.
        /// </summary>
        public int Position { get; private set; }

        /// <summary>
        ///     The current value of the first input.
        /// </summary>
        public bool Input1 { get; private set; }

        /// <summary>
        ///     The current value of the second input.
        /// </summary>
        public bool Input2 { get; private set; }

        /// <summary>
        ///     The current value of the third input.
        /// </summary>
        public bool Input3 { get; private set; }

        /// <summary>
        ///     The current value of the first output.
        /// </summary>
        public bool Output1 { get; private set; }

        /// <summary>
        ///     The current value of the second output.
        /// </summary>
        public bool Output2 { get; private set; }

        /// <summary>
        ///     The current value of the third output.
        /// </summary>
        public bool Output3 { get; private set; }

        /// <summary>
        ///     Indicates whether this <see cref="SaciaBoard"/> is disabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if disabled; otherwise, <c>false</c>.
        /// </value>
        public bool Disabled { get; private set; }

        /// <summary>
        ///     Indicates whether [speed set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [speed set]; otherwise, <c>false</c>.
        /// </value>
        public bool SpeedSet { get; private set; }

        /// <summary>
        ///     Indicates whether [current set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [current set]; otherwise, <c>false</c>.
        /// </value>
        public bool CurrentSet { get; private set; }

        /// <summary>
        ///     Indicates whether [home set].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [home set]; otherwise, <c>false</c>.
        /// </value>
        public bool HomeSet { get; private set; }

        /// <summary>
        ///     Indicates whether [over temperature].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [over temperature]; otherwise, <c>false</c>.
        /// </value>
        public bool OverTemperature { get; private set; }

        /// <summary>
        ///     Indicates whether the <see cref="SaciaBoard"/> motor is running.
        /// </summary>
        /// <value>
        ///   <c>true</c> if motor running; otherwise, <c>false</c>.
        /// </value>
        public bool Running { get; private set; }

        public event InputChangedEventHandler InputChanged;
        public event OutputChangedEventHandler OutputChanged;
        public event MotorStartedEventHandler MotorStarted;
        public event MotorStoppedEventHandler MotorStopped;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SaciaBoard"/> class.
        /// </summary>
        /// <param name="sequenceNumber">
        ///     The CAN board sequence number as an integer 
        ///     representation of its binary dip switch position
        /// </param>
        /// <param name="controller">
        ///     The <see cref="ICanController"/> associated with this board. 
        /// </param>
        public SaciaBoard(int sequenceNumber, ICanController controller) 
            : base(sequenceNumber, controller)
        { }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to enable this board.
        /// </summary>
        public void Enable()
        {
            byte[] data = new byte[8] { 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to set the 
        ///     board motor positions to zero.
        /// </summary>
        public void Zero()
        {
            byte[] data = new byte[8] { 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to reset 
        ///     this <see cref="SaciaBoard"/> state.
        /// </summary>
        public void Reset()
        {
            byte[] data = new byte[8] { 0x07, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to set the motor 
        ///     speed, acceleration and deceleration.
        /// </summary>
        /// <param name="speed">The motor speed.</param>
        /// <param name="acceleration">The motor acceleration.</param>
        /// <param name="deceleration">The motor decceleration</param>
        public void SetMovementProperties(int speed, int acceleration, int deceleration)
        {
            byte[] data = new byte[8] { 0x02, 0x00, GetByte(speed, 0), GetByte(speed, 1), GetByte(acceleration, 0), GetByte(acceleration, 1), GetByte(deceleration, 0), GetByte(deceleration, 1), };
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

        /// <summary>
        /// Polls this instance.
        /// </summary>
        public void Poll()
        {
            byte[] data = new byte[8] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        /// Nones this instance.
        /// </summary>
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

        /// <summary>
        ///     Parse the specified CAN data
        /// </summary>
        /// <param name="data">The data to parse.</param>
        protected internal override void Parse(byte[] data)
        {
            Position = (((int)data[3]) << 32) | (((int)data[2]) << 16) | (((int)data[1]) << 8) | (int)data[0];

            Disabled = (data[5] & 0x80) > 0;
            SpeedSet = !((data[5] & 0x40) > 0);
            CurrentSet = !((data[5] & 0x20) > 0);
            HomeSet = !((data[5] & 0x10) > 0);
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

            if (Running != (data[5] & 0x01) > 0)
            {
                Running = (data[5] & 0x01) > 0;
                if (Running)
                {
                    MotorStarted?.Invoke(this);
                }
                else
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
