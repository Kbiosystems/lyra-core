﻿#if !NET40
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LyraElectronics.Extensions
{
    internal static class TaskExtensions
    {
        internal static async Task<TEventArgs> FromEvent<TEventHandler, TEventArgs>(
          Func<Action<TEventArgs>, Action, Action<Exception>, TEventHandler> getHandler,
          Action<TEventHandler> subscribe,
          Action<TEventHandler> unsubscribe,
          Action<Action<TEventArgs>, Action, Action<Exception>> initiate,
          CancellationToken token) where TEventHandler : class
        {
            var tcs = new TaskCompletionSource<TEventArgs>();

            Action<TEventArgs> complete = (args) => tcs.TrySetResult(args);
            Action cancel = () => tcs.TrySetCanceled();
            Action<Exception> reject = (ex) => tcs.TrySetException(ex);

            TEventHandler handler = getHandler(complete, cancel, reject);

            subscribe(handler);
            try
            {
                using (token.Register(() => tcs.TrySetCanceled()))
                {
                    initiate(complete, cancel, reject);
                    return await tcs.Task;
                }
            }
            finally
            {
                unsubscribe(handler);
            }
        }
    }
}
#endif