
public class Item : Component
{
    public ITEM item = ITEM.Error;
    public ITEMFLAG flags = ITEMFLAG.NONE;
    public EQUIPSLOT equipslot = EQUIPSLOT.NONE;
    public string name = "Error";
    public string description = "Default description";
    public int weight = 0;
    public int value = 0;
    public uint owner = 0;
}
