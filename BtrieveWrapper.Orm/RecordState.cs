namespace BtrieveWrapper.Orm
{
    public enum RecordState : byte
    {
        Added,
        Modified,
        Deleted,
        Unchanged,
        Detached,
        Incomplete
    }
}
