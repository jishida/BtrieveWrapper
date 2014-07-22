namespace BtrieveWrapper
{
    /// <summary>
    /// Openオペレーションの引数を取得する。
    /// </summary>
    public enum OpenMode : sbyte
    {
        Normal = 0,
        Accelerated = -1,
        ReadOnly = -2,
        Verify = -3,
        Exclusive = -4,
        SingleEngin = -32,
        MultiEngin = -64
    }
}