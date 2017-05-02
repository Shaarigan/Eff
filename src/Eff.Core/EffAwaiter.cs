﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eff.Core
{
    public struct EffAwaiter<TResult> : ICriticalNotifyCompletion
    {
        private readonly Eff<TResult> eff;
   
        internal EffAwaiter(Eff<TResult> eff)
        {
            this.eff = eff;
        }

        public bool IsCompleted => eff.IsCompleted;
        public TResult GetResult() => eff.Result;
        

        public void OnCompleted(Action continuation)
        {
            eff.AsTask().ConfigureAwait(continueOnCapturedContext: true).GetAwaiter().OnCompleted(continuation);
        }
        public void UnsafeOnCompleted(Action continuation)
        {
            eff.AsTask().ConfigureAwait(continueOnCapturedContext: true).GetAwaiter().UnsafeOnCompleted(continuation);
        }
    }
}
