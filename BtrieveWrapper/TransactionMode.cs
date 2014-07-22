namespace BtrieveWrapper
{
    public enum TransactionMode : ushort
    {
        Exclusive = 0,
        Concurrency = 1000,
        ConcurryncyNoRetry = 1500
    }
}
