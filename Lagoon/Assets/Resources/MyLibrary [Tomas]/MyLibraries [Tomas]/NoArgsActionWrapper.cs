using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
 * These classes are used when the paremeters of a delegate are known and won't change and so they can act as argumentless actions.
 * It can also be used to allow a delegate with arguments to be grouped with argumentless delegates
 */
public class NoArgsActionWrapper<T1>
{
     public readonly System.Action wrappedAction;

     System.Action<T1> realAction;
     public T1 arg1;
    public NoArgsActionWrapper(System.Action<T1>  action_, T1 arg1_)
    {
        wrappedAction = SafeInvoke;
        realAction = action_;
        arg1 = arg1_;       
    }
    public void SafeInvoke()
    {
        realAction?.Invoke(arg1);
    } 
}



//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1> realAction;
//    public T1 arg1;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_)
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}
//public class NoArgsActionWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16>
//{
//    public readonly System.Action wrappedAction;

//    System.Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15, T16> realAction;
//    public T1 arg1;
//    public T2 arg2;
//    public T3 arg3;
//    public T4 arg4;
//    public T5 arg5;
//    public T6 arg6;
//    public T7 arg7;
//    public T8 arg8;
//    public T9 arg9;
//    public T10 arg10;
//    public T11 arg11;
//    public T12 arg12;
//    public T13 arg13;
//    public T14 arg14;
//    public T15 arg15;
//    public T16 arg16;
//    public NoArgsActionWrapper(System.Action<T1> action_, T1 arg1_, T1 arg1_, T1 arg1_, T1 arg1_, T4 arg5_, T6 arg6_, T7 arg7_, T8 arg8_, T9 arg9_, T10 arg10_, T11 arg11_, T12 arg12_, T13 arg13_, T14 arg14_, T15 arg15_, T16 arg16_, )
//    {
//        wrappedAction = SafeInvoke;
//        realAction = action_;
//        arg1 = arg1_;
//    }
//    public void SafeInvoke()
//    {
//        realAction?.Invoke(arg1);
//    }
//}