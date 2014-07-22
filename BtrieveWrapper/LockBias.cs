namespace BtrieveWrapper
{
    public enum LockBias : ushort
    {
        None = 0,
        SingleRecordWaitLock = 100,
        SingleRecordNoWaitLock = 200,
        MultiRecordWaitLock = 300,
        MultiRecordNoWaitLock = 400
    }
}
