
using System;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

public class MonData
{

    public static void init()
    {
        Type[] types = new Type[] { typeof(Glyph), typeof(Creature), typeof(Defenses), typeof(Attacks), typeof(Attack) };
        XmlSerializer serializer = new XmlSerializer(typeof(monsterentry[]), types);
        TextReader reader = new StreamReader(".\\Assets\\Scripts\\Data\\creatures.xml");
        monsterentry[] e = (monsterentry[]) serializer.Deserialize(reader);
        entries = new monsterentry[e.Length];
        for (int i = 0; i < e.Length; i++)
        {
            entries[i] = e[i];
        }
        reader.Close();

    }

    public static monsterentry[] entries;

    public static monsterentry GetMonsterEntry(MON id)
    {
        foreach (monsterentry entry in entries)
        {
            if (entry.mid == id) return entry;
        }

        return entries[0];
    }
}


public struct monsterentry
{
    public MON mid;
    public object[] data;
}