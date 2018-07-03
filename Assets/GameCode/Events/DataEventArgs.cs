using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataEventArgs<T> : EventArgs
{
    public T Data;

    public DataEventArgs()
    {
        Data = default(T);
    }

    public DataEventArgs(T data)
    {
        Data = data;
    }
}
