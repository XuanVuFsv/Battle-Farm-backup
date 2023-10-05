using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyDebug: Singleton<MyDebug>
{
    public bool isActive = true;

    public void Log(object message, Object context)
    {
        if (!isActive) return;
        Debug.Log(message, context);
    }
    
    public void Log(object message)
    {
        if (!isActive) return;
        Debug.Log(message);
    }
}
