public class Data<T0>
{
    public T0 Arg0;

    public Data(T0 arg0)
    {
        Arg0 = arg0;
    }
}

public class Data<T0, T1> : Data<T0>
{
    public T1 Arg1;

    public Data(T0 arg0, T1 arg1) : base(arg0)
    {
        Arg1 = arg1;
    }
}

public class Data<T0, T1, T2> : Data<T0, T1>
{
    public T2 Arg2;

    public Data(T0 arg0, T1 arg1, T2 arg2) : base(arg0, arg1)
    {
        Arg2 = arg2;
    }
}