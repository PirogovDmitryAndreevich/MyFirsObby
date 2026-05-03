using System;

[Serializable]
public class ForOpener
{
    public OpeningType Type;
    public int UnblockPrice;
    public OpeningType[] Next;
}