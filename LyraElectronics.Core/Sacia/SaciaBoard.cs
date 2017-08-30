using LyraElectronics.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LyraElectronics.Sacia
{
    /// <summary>
    ///     An implementation of the <see cref="CanBoard"/> 
    ///     class for the Sacia hardware.
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
        /// Gets the inputs.
        /// </summary>
        /// <value>
        /// The inputs.
        /// </value>
        public SaciaInputs Inputs { get; private set; }

        /// <summary>
        /// Gets the outputs.
        /// </summary>
        /// <value>
        /// The outputs.
        /// </value>
        public SaciaOutputs Outputs { get; private set; }

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

        /// <summary>
        ///     Occurs when [input changed].
        /// </summary>
        public event EventHandler<InputChangedEventArgs> InputChanged;

        /// <summary>
        ///     Occurs when [output changed].
        /// </summary>
        public event EventHandler<OutputChangedEventArgs> OutputChanged;

        /// <summary>
        ///     Occurs when [motor started].
        /// </summary>
        public event EventHandler<MotorStartedEventArgs> MotorStarted;

        /// <summary>
        ///     Occurs when [motor stopped].
        /// </summary>
        public event EventHandler<MotorStoppedEventArgs> MotorStopped;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SaciaBoard"/> class.
        /// </summary>
        /// <param name="sequenceNumber">
        ///     The CAN board sequence number as an integer 
        ///     representation of its binary dip switch position
        /// </param>
        /// <param name="controller">
        ///     The <see cref="CanController"/> associated with this board. 
        /// </param>
        public SaciaBoard(int sequenceNumber, CanController controller) 
            : base(sequenceNumber, controller)
        {
            Inputs = new SaciaInputs();
            Outputs = new SaciaOutputs();
        }

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
        ///     Send <see cref="CanMessage"/> to set 
        ///     the motor hold and run currents
        /// </summary>
        /// <param name="runCurrent">The motor run current.</param>
        /// <param name="holdCurrent">The motor hold current.</param>
        public void SetCurrent(int runCurrent, int holdCurrent)
        {
            byte[] data = new byte[8] { 0x03, 0x00, GetByte(holdCurrent, 0), GetByte(holdCurrent, 1), GetByte(runCurrent, 0), GetByte(runCurrent, 1), 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to set 
        ///     the specified output to on/off
        /// </summary>
        /// <param name="output">The output to set</param>
        /// <param name="value">The output value. If true, on, else off</param>
        public void SetOutput(int output, bool value)
        {
            bool out1 = output == 0 ? value : Outputs.Output1;
            bool out2 = output == 1 ? value : Outputs.Output2;
            bool out3 = output == 2 ? value : Outputs.Output3;

            SetOutputs(out1, out2, out3);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to set 
        ///     all outputs to on/off
        /// </summary>
        /// <param name="value">The output value. If true, on, else off</param>
        public void SetAllOutputs(bool value)
        {
            SetOutputs(value, value, value);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to set 
        ///     the specified outputs to on/off
        /// </summary>
        /// <param name="out1">The output value for output 1. If true, on, else off</param>
        /// <param name="out2">The output value for output 2. If true, on, else off</param>
        /// <param name="out3">The output value for output 3. If true, on, else off</param>
        public void SetOutputs(bool out1, bool out2, bool out3)
        {
            byte[] data = new byte[8] { 0x04, 0x00, (byte)(out1 ? 0x01 : 0x00), (byte)(out2 ? 0x01 : 0x00), (byte)(out3 ? 0x01 : 0x00), 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to poll this board.
        /// </summary>
        public void Poll()
        {
            byte[] data = new byte[8] { 0x0A, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to reset all inputs/outputs
        /// </summary>
        public void None()
        {
            byte[] data = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to 
        ///     goto the specified motor position.
        /// </summary>
        /// <param name="position">The motor position to goto.</param>
        public virtual void Goto(int position)
        {
            Debug.WriteLine(GetByte(position, 0).ToString("X1") + GetByte(position, 1).ToString("X1") + GetByte(position, 2).ToString("X1") + GetByte(position, 3).ToString("X1"));
            byte[] data = new byte[8] { 0x01, 0x00, GetByte(position, 0), GetByte(position, 1), GetByte(position, 2), GetByte(position, 3), 0x0, 0x0 };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to run the 
        ///     motor for a specified number of steps.
        /// </summary>
        /// <param name="steps">The number of steps to run.</param>
        public void Run(int steps)  
        {
            Run(steps, -1, false);          
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to run the motor
        ///     until an input is set to specified value or 
        ///     to a maximum number of steps.
        /// </summary>
        /// <param name="stopInput">The input to watch.</param>
        /// <param name="stopLogic">The value of the input to stop on.</param>
        /// <param name="steps">The maximum steps to run.</param>
        public void Run(int steps, int stopInput, bool stopLogic)
        {
            byte direction = 0x00;
            if (steps < 0)
            {
                direction = 0x01;
                steps *= -1;
            }
            Debug.WriteLine(GetByte(steps, 0).ToString("X2") + GetByte(steps, 1).ToString("X2") + GetByte(steps, 2).ToString("X2") + GetByte(steps, 3).ToString("X2"));

            int logic = stopLogic ? 0x01 : 0x00;

            byte[] data = new byte[8] { 0x08, direction, Convert.ToByte(stopInput + 1), Convert.ToByte(logic), GetByte(steps, 0), GetByte(steps, 1), GetByte(steps, 2), GetByte(steps, 3), };
            SendMessage(data);
        }

        /// <summary>
        ///     Send <see cref="CanMessage"/> to stop the motor. Terminates movement.
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
            if (data[3] > 0x7F)
            {
                Position = -((~data[3] & 0x000000FF) << 32 | (~data[2] & 0x000000FF) << 16 | (~data[1] & 0x000000FF) << 8 | (~data[0] & 0x000000FF));
            }
            else
            {
                Position = data[3] << 32 | data[2] << 16 | data[1] << 8 | data[0];
            }

            Disabled = (data[5] & 0x80) > 0;
            SpeedSet = !((data[5] & 0x40) > 0);
            CurrentSet = !((data[5] & 0x20) > 0);
            HomeSet = !((data[5] & 0x10) > 0);
            OverTemperature = (data[5] & 0x82) > 0;

            // set inputs and invoke input changed event where applicable
            foreach (var args in Inputs.Parse(data[4]))
            {
                InputChanged?.Invoke(this, args);
            }

            // set outputs and invoke output changed event where applicable
            foreach (var args in Outputs.Parse(data[6]))
            {
                OutputChanged?.Invoke(this, args);
            }

            // set running value and invoke stop/start events
            if (Running != (data[5] & 0x01) > 0)
            {
                Running = (data[5] & 0x01) > 0;
                if (Running)
                {
                    MotorStarted?.Invoke(this, new MotorStartedEventArgs());
                }
                else
                {
                    MotorStopped?.Invoke(this, new MotorStoppedEventArgs());
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
