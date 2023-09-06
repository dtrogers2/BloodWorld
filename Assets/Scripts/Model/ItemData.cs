
public class ItemData 
{
    public static itementry[] entries = new itementry[]
    {
        new itementry {id = ITEM.Error, data = new object[] { new Item { description = "ERROR" } }, components = new COMPONENT[] { COMPONENT.ITEM} },
    };

    public static itementry GetItemEntry(ITEM id)
    {
        foreach (itementry entry in entries)
        {
            if (entry.id == id) return entry;
        }

        return entries[0];
    }
}

public struct itementry
{
    public ITEM id;
    public object[] data;
    public COMPONENT[] components;
}
