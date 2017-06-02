﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Eff.Core
{
    public class EffMethodBuilder<TResult>
    {
        private Eff<TResult> eff;
        private IAsyncStateMachine stateMachine;
        private (string name, FieldInfo fieldInfo)[] parametersInfo;
        private (string name, FieldInfo fieldInfo)[] localVariablesInfo;

        public static EffMethodBuilder<TResult> Create()
        {
            return new EffMethodBuilder<TResult>();
        }

        public void Start<TStateMachine>(ref TStateMachine stateMachine) where TStateMachine : IAsyncStateMachine
        {
            this.stateMachine = stateMachine;
        }

        public void SetStateMachine(IAsyncStateMachine stateMachine)
        {
            
        }

        public void SetResult(TResult result)
        {
            this.eff = new SetResult<TResult>(result);
        }

        public void SetException(Exception exception)
        {
            this.eff = new SetException<TResult>(exception);
        }

        public Eff<TResult> Task => this.eff;
        
        public void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            AwaitOnCompleted(ref awaiter, ref stateMachine, true);
        }


        [SecuritySafeCritical]
        public void AwaitUnsafeOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine)
            where TAwaiter : ICriticalNotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {
            AwaitOnCompleted(ref awaiter, ref stateMachine, false);
        }


        private static (string name, object value)[] GetParameters((string name, FieldInfo fieldInfo)[] parametersInfo, IAsyncStateMachine _stateMachine)
        {
            return parametersInfo.Select(parameter => (parameter.name, parameter.fieldInfo.GetValue(_stateMachine)))
                                 .ToArray();
        }

        private static (string name, object value)[] GetLocalVariables((string name, FieldInfo fieldInfo)[] localVariablesInfo, IAsyncStateMachine _stateMachine)
        {
            return localVariablesInfo.Select(local => (local.name, local.fieldInfo.GetValue(_stateMachine)))
                                     .ToArray();
        }

        private void AwaitOnCompleted<TAwaiter, TStateMachine>(ref TAwaiter awaiter, ref TStateMachine stateMachine, bool safe)
            where TAwaiter : INotifyCompletion
            where TStateMachine : IAsyncStateMachine
        {

            switch (awaiter)
            {
                case IEffect effect:

                    this.eff = effect.Await<TResult>(() =>
                    {
                        this.stateMachine.MoveNext();
                        return this.eff;
                    });

                    //if ((effect.CaptureState || handler.EnableParametersLogging) && this.parametersInfo == null)
                    //{
                    //    var fieldInfos = stateMachine.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    //    this.parametersInfo = fieldInfos.Where(fieldInfo => !fieldInfo.Name.StartsWith("<"))
                    //                               .Select(fieldInfo => (fieldInfo.Name, fieldInfo))
                    //                               .ToArray();
                    //}
                    //if ((effect.CaptureState || handler.EnableLocalVariablesLogging) && this.localVariablesInfo == null)
                    //{
                    //    var fieldInfos = stateMachine.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                    //    this.localVariablesInfo = fieldInfos.Where(fieldInfo => !fieldInfo.Name.StartsWith("<>"))
                    //                                   .Where(fieldInfo => fieldInfo.Name.StartsWith("<"))
                    //                                   .Select(fieldInfo => (fieldInfo.Name.Substring(1, fieldInfo.Name.LastIndexOf(">") - 1), fieldInfo))
                    //                                   .ToArray();
                    //}

                    //(string name, FieldInfo fieldInfo)[] parametersInfo = this.parametersInfo;
                    //(string name, FieldInfo fieldInfo)[] localVariablesInfo = this.localVariablesInfo;

                    //async ValueTask<ValueTuple> ApplyEffectHandler(IEffectHandler _handler, IAsyncStateMachine _stateMachine)
                    //{
                    //    try
                    //    {
                    //        await effect.Accept(_handler);
                    //        if (!effect.IsCompleted)
                    //            throw new EffException($"Effect {effect.GetType().Name} is not completed.");
                    //        return ValueTuple.Create();
                    //    }
                    //    catch (AggregateException ex)
                    //    {
                    //        effect.SetException(ex.InnerException);
                    //        return ValueTuple.Create();
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        effect.SetException(ex);
                    //        return ValueTuple.Create();
                    //    }
                    //    finally
                    //    {
                    //        if (_handler.EnableExceptionLogging && effect.Exception != null)
                    //        {
                    //            await _handler.Log(new ExceptionLog
                    //            {
                    //                CallerFilePath = effect.CallerFilePath,
                    //                CallerLineNumber = effect.CallerLineNumber,
                    //                CallerMemberName = effect.CallerMemberName,
                    //                Exception = effect.Exception,
                    //                Parameters = _handler.EnableParametersLogging ? GetParameters(parametersInfo, _stateMachine) : null,
                    //                LocalVariables = _handler.EnableLocalVariablesLogging ? GetLocalVariables(localVariablesInfo, _stateMachine) : null,
                    //            });
                    //        }
                    //        if (_handler.EnableTraceLogging && effect.HasResult)
                    //        {
                    //            await _handler.Log(new ResultLog
                    //            {
                    //                CallerFilePath = effect.CallerFilePath,
                    //                CallerLineNumber = effect.CallerLineNumber,
                    //                CallerMemberName = effect.CallerMemberName,
                    //                Result = effect.Result,
                    //                Parameters = _handler.EnableParametersLogging ? GetParameters(parametersInfo, _stateMachine) : null,
                    //                LocalVariables = _handler.EnableLocalVariablesLogging ? GetLocalVariables(localVariablesInfo, _stateMachine) : null,
                    //            });
                    //        }
                    //        await System.Threading.Tasks.Task.Yield();
                    //        EffectExecutionContext.Handler = _handler; // restore EffectHandler
                    //    }
                    //}
                    //IAsyncStateMachine boxedStateMachine = effect.CaptureState || handler.EnableParametersLogging || handler.EnableLocalVariablesLogging ? stateMachine : default(TStateMachine);
                    //if (effect.CaptureState)
                    //{
                    //    var parameters = GetParameters(parametersInfo, boxedStateMachine);
                    //    var localVariables = GetLocalVariables(localVariablesInfo, boxedStateMachine);
                    //    effect.SetState(parameters, localVariables);
                    //}
                    //var task = ApplyEffectHandler(handler, boxedStateMachine);
                    
                    //var _awaiter = task.GetAwaiter();
                    //if (safe)
                    //    methodBuilder.AwaitOnCompleted(ref _awaiter, ref stateMachine);
                    //else
                    //    methodBuilder.AwaitUnsafeOnCompleted(ref _awaiter, ref stateMachine);
                    
                    break;
                default:
                    throw new EffException($"Awaiter {awaiter.GetType().Name} is not an effect. Try to use obj.AsEffect().");
            }
        }


    }

}
