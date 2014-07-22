namespace BtrieveWrapper
{
    public enum KeyFlag : ushort
    {
        None = 0,
        Dup = 1,
        Mod = 2,
        Bin = 4,
        Nul = 8,
        Seg = 16,
        Alt = 32,
        NumberedACS = 1056,
        NamedACS = 3104,
        DescKey = 64,
        RepeatDupsKey = 128,
        ExttypeKey = 256,
        ManualKey = 512,
        NocaseKey = 1024
    }
}
