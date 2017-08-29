﻿#if !NET40

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
}
#endif
