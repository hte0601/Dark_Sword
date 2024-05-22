using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourExt : MonoBehaviour
{
    protected Coroutine DelayInvoke(float delaySeconds, Action function)
    {
        return StartCoroutine(DelayInvokeCoroutine(delaySeconds, function));
    }

    private IEnumerator DelayInvokeCoroutine(float delaySeconds, Action function)
    {
        yield return new WaitForSeconds(delaySeconds);

        function.Invoke();
    }


    protected Coroutine DelayInvoke<T>(float delaySeconds, Action<T> function, T arg)
    {
        return StartCoroutine(DelayInvokeCoroutine(delaySeconds, function, arg));
    }

    private IEnumerator DelayInvokeCoroutine<T>(float delaySeconds, Action<T> function, T arg)
    {
        yield return new WaitForSeconds(delaySeconds);

        function.Invoke(arg);
    }


    protected Coroutine DelayInvoke<T1, T2>(float delaySeconds, Action<T1, T2> function, T1 arg1, T2 arg2)
    {
        return StartCoroutine(DelayInvokeCoroutine(delaySeconds, function, arg1, arg2));
    }

    private IEnumerator DelayInvokeCoroutine<T1, T2>(float delaySeconds, Action<T1, T2> function, T1 arg1, T2 arg2)
    {
        yield return new WaitForSeconds(delaySeconds);

        function.Invoke(arg1, arg2);
    }


    protected Coroutine DelayInvoke<T1, T2, T3>(float delaySeconds, Action<T1, T2, T3> function, T1 arg1, T2 arg2, T3 arg3)
    {
        return StartCoroutine(DelayInvokeCoroutine(delaySeconds, function, arg1, arg2, arg3));
    }

    private IEnumerator DelayInvokeCoroutine<T1, T2, T3>(float delaySeconds, Action<T1, T2, T3> function, T1 arg1, T2 arg2, T3 arg3)
    {
        yield return new WaitForSeconds(delaySeconds);

        function.Invoke(arg1, arg2, arg3);
    }
}
