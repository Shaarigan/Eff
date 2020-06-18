﻿using System;
using System.Runtime.CompilerServices;
using Nessos.Effects.Builders;
using Nessos.Effects.Handlers;

namespace Nessos.Effects
{
    /// <summary>
    ///   Represents an abstract effect returning no result.
    /// </summary>
    public abstract class Effect : Effect<Unit>
    {

    }

    /// <summary>
    ///   Represents an abstract effect.
    /// </summary>
    /// <typeparam name="TResult">Return type of the abstract effect.</typeparam>
    public abstract class Effect<TResult>
    {
        public Awaiter<TResult> GetAwaiter() => new EffectAwaiter<TResult>(this);

        /// <summary>
        ///   Configures an EffAwaiter instance with supplied parameters.
        /// </summary>
        /// <param name="callerMemberName"></param>
        /// <param name="callerFilePath"></param>
        /// <param name="callerLineNumber"></param>
        /// <returns>An EffAwaiter instance with callsite metadata.</returns>
        public Awaiter<TResult> ConfigureAwait(
            [CallerMemberName] string callerMemberName = "",
            [CallerFilePath] string callerFilePath = "",
            [CallerLineNumber] int callerLineNumber = 0)
        {
            return new EffectAwaiter<TResult>(this)
            {
                CallerMemberName = callerMemberName,
                CallerLineNumber = callerLineNumber,
                CallerFilePath = callerFilePath
            };
        }
    }
}