﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Eff.Core
{
    public interface IEffect : ICriticalNotifyCompletion
    {
        bool IsCompleted { get; }

        string CallerMemberName { get; }
        string CallerFilePath { get; }
        string CallerLineNumber { get; }
    }

    public interface IEffect<TResult> : IEffect
    {
        TResult GetResult();
    }
}