
using System;
using System.IO;
using System.Xml.Serialization;

public class ItemData 
{
    public static itementry[] entries = new itementry[0];

    public static void init()
    {
        Type[] types = new Type[] { typeof(Glyph), typeof(Creature), typeof(Defenses), typeof(Attacks), typeof(Attack), typeof(Ego), typeof(Item) };
        XmlSerializer serializer = new XmlSerializer(typeof(itementry[]), types);
        TextReader reader = new StreamReader(".\\Assets\\Scripts\\Data\\items.xml");
        itementry[] e = (itementry[])serializer.Deserialize(reader);
        entries = new itementry[e.Length];
        for (int i = 0; i < e.Length; i++)
        {
            entries[i] = e[i];
        }
        reader.Close();

    }
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
    public Component[] components;
}
