#if !NET40

using LyraElectronics.Events;
using LyraElectronics.Sacia;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LyraElectronics.Extensions
{
    /// <summary>
    ///     A set of extension methods for the <see cref="SaciaBoard"/> class
    /// </summary>
    public static class SaciaBoardExtensions
    {
        /// <summary>
        ///     Sets the zero-based output of the given <see cref="SaciaBoard"/>
        ///     to the specified value. Exits when confirmation recieved from
        ///     the specifed board that the output has been set.
        /// </summary>
        /// <param name="board">The <see cref="SaciaBoard"/></param>
        /// <param name="output">The zero-based output to set.</param>
        /// <param name="value">The value to set the output to.</param>
        /// <returns></returns>
        public static async Task SetOutputSafe(this SaciaBoard board, int output, bool value)
        {
            if (board.Outputs[output] != value)
            {
                var wait = board.WaitForOutput(output, value, CancellationToken.None, TimeSpan.FromSeconds(3));

                board.SetOutput(output, value);

                await wait.ConfigureAwait(false);
            }
        }

        /// <summary>
        ///     Wait for the given <see cref="SaciaBoard"/> to stop.
        /// </summary>
        /// <param name="board">The <see cref="SaciaBoard"/></param>
        /// <param name="token">The cancellation token.</param>
        /// <param name="timeout">The time to wait. If time reach <see cref="TimeoutException"/> will be thrown</param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">The operation has timed out waiting for the motor to complete.</exception>
        public static async Task WaitForStopped(this SaciaBoard board, CancellationToken token, TimeSpan timeout)
        {       
            var task = TaskExtensions.FromEvent<EventHandler<MotorStoppedEventArgs>, MotorStoppedEventArgs>(
                (complete, cancel, reject) => // get handler
                        (sender, args) => complete(args),
                    handler => // subscribe
                        board.MotorStopped += handler,
                    handler => // unsubscribe
                        board.MotorStopped -= handler,
                    (complete, cancel, reject) => // start the operation
                    { },
                    token);

            if (await Task.WhenAny(task, Task.Delay(timeout)).ConfigureAwait(false) == task)
            {
                // re-await so any cancellations or exceptions can be rethrown
                await task;
            }
            else
            {
                throw new TimeoutException("The operation has timed out waiting for the motor to complete.");
            }

        }

        /// <summary>
        ///     Wait for the given <see cref="SaciaBoard"/> input 
        ///     to change to the expected value.
        /// </summary>
        /// <param name="board">The <see cref="SaciaBoard"/></param>
        /// <param name="input">The input to wait for</param>
        /// <param name="value">The expected value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <param name="timeout">The time to wait. If time reach <see cref="TimeoutException"/> will be thrown</param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">The operation has timed out waiting for an input to be set.</exception>
        public static async Task WaitForInput(this SaciaBoard board, int input, bool value, CancellationToken token, TimeSpan timeout)
        {
            if (board.Inputs[input] != value)
            {
                bool complete = false;
                board.InputChanged += (o, e) =>
                {
                    if (e.Input == input && e.NewValue == value)
                    {
                        complete = true;
                    }
                };

                DateTime start = DateTime.UtcNow;
                while (!complete)
                {
                    if (DateTime.UtcNow > start + timeout)
                    {
                        throw new TimeoutException("The operation has timed out waiting for an input to be set.");
                    }
                    await Task.Delay(100).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Wait for the given <see cref="SaciaBoard"/> output 
        ///     to change to the expected value.
        /// </summary>
        /// <param name="board">The <see cref="SaciaBoard"/></param>
        /// <param name="output">The output to wait for.</param>
        /// <param name="value">The expected value.</param>
        /// <param name="token">The cancellation token.</param>
        /// <param name="timeout">The time to wait. If time reach <see cref="TimeoutException"/> will be thrown</param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">The operation has timed out waiting for an output to be set.</exception>
        public static async Task WaitForOutput(this SaciaBoard board, int output, bool value, CancellationToken token, TimeSpan timeout)
        {
            if (board.Outputs[output] != value)
            {
                bool complete = false;
                board.OutputChanged += (o, e) =>
                {
                    if (e.Output == output && e.NewValue == value)
                    {
                        complete = true;
                    }
                };

                DateTime start = DateTime.UtcNow;
                while (!complete)
                {
                    if (DateTime.UtcNow > start + timeout)
                    {
                        throw new TimeoutException("The operation has timed out waiting for an output to be set.");
                    }
                    await Task.Delay(100).ConfigureAwait(false);
                }
            }
        }

        /// <summary>
        ///     Wait for the given <see cref="SaciaBoard"/> motor
        ///     to reach its stopped position.
        /// </summary>
        /// <param name="board">The <see cref="SaciaBoard"/></param>
        /// <param name="distance">The distance to run the motor</param>
        /// <param name="timeout">The time to wait. If time reach <see cref="TimeoutException"/> will be thrown</param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">The operation has timed out waiting for an output to be set.</exception>
        public static async Task RunUntilStopped(this SaciaBoard board, int distance, TimeSpan timeout)
        {
            var expectedPos = board.Position + distance;

            var waitForStopped = board.WaitForStopped(CancellationToken.None, timeout);
            var waitForPos = board.WaitForPositionReached(expectedPos, timeout);

            board.Run(distance);

            var task = await Task.WhenAny(waitForStopped, waitForPos).ConfigureAwait(false);

            // re-await so any cancellations or exceptions can be rethrown
            await task;           
        }

        /// <summary>
        ///     Wait for the given <see cref="SaciaBoard"/> motor
        ///     to reach its stopped position.
        /// </summary>
        /// <param name="board">The <see cref="SaciaBoard"/></param>
        /// <param name="maxDistance">The maximum distance to run the motor.</param>
        /// <param name="input">The input to wait for.</param>
        /// <param name="value">The expected value of the input.</param>
        /// <param name="timeout">The time to wait. If time reach <see cref="TimeoutException"/> will be thrown</param>
        /// <param name="errorIfMaxDistanceReached">
        ///     If true, throw <see cref="InvalidOperationException"/> 
        ///     if max distance reached before input detected
        /// </param>
        /// <returns></returns>
        /// <exception cref="System.TimeoutException">The operation has timed out waiting for an output to be set.</exception>
        public static async Task RunUntilStopped(this SaciaBoard board, int maxDistance, int input, bool value, TimeSpan timeout, bool errorIfMaxDistanceReached = true)
        {
            var maxPos = board.Position + maxDistance;

            var waitForInput = board.WaitForInput(input, value, CancellationToken.None, timeout);
            var motorStopped = board.WaitForStopped(CancellationToken.None, timeout);
            var waitForPos = board.WaitForPositionReached(maxPos, timeout);

            board.Run(maxDistance, input, value);

            var task = await Task.WhenAny(waitForInput, motorStopped, waitForPos);

            if (board.Inputs[input] != value)
            {
                if (board.Position == maxPos && errorIfMaxDistanceReached)
                {
                    throw new InvalidOperationException("Max distance reached before input detected.");
                }
            }

            await task;
        }

        /// <summary>
        ///     Wait for the given <see cref="SaciaBoard"/> motor
        ///     to reach its stopped position.
        /// </summary>
        /// <param name="board">The <see cref="SaciaBoard"/></param>
        /// <param name="timeout">The time to wait for the zero value to be set. If time reach <see cref="TimeoutException"/> will be thrown</param>
        /// <exception cref="System.TimeoutException">The operation has timed out waiting for a zero value to be set.</exception>
        public static async Task ZeroSafe(this SaciaBoard board, TimeSpan timeout)
        {
            board.Zero();

            var start = DateTime.UtcNow;
            while (board.Position != 0)
            {
                if (start + timeout < DateTime.UtcNow)
                {
                    throw new InvalidOperationException("The operation has timed out waiting for a zero value to be set.");
                }
                await Task.Delay(200);
            }
        }

        private static async Task WaitForPositionReached(this SaciaBoard board, double motorPosition, TimeSpan timeout)
        {
            DateTime start = DateTime.UtcNow;
            while (board.Position > motorPosition + 1 || board.Position < motorPosition - 1) // account for potential half step rounding
            {
                if (start + timeout < DateTime.UtcNow)
                {
                    throw new TimeoutException("The operation has timed out waiting for the motor to complete.");
                }
                await Task.Delay(20);
            }
        }
    }
}
#endif
