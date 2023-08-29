using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonData
{
    public static monsterentry[] entries = new monsterentry[]
    {
        new monsterentry { mid = MONTYPE.ERROR, basechar = 'E', color = ColorHex.Red,
            HD = 0, avg_hp = 0, AC = 0, EV = 0, REF = 0, WIL = 0, FOR = 0, LUK = 0,baseMovSpeed = 1, baseAtkSpeed = 1,
            faction = FacType.HUMANS,
            parts = PartDataUtils.HUMANPARTS(),
        },
        new monsterentry { mid = MONTYPE.HUMAN, basechar = 'h', color = ColorHex.White,
            HD = 1, avg_hp = 10, AC = 0, EV = 0, REF = 0, WIL = 0, FOR = 0, LUK = 0,baseMovSpeed = 1, baseAtkSpeed = 1,
            faction = FacType.HUMANS,
            parts = PartDataUtils.HUMANPARTS(),
        },
        new monsterentry { mid = MONTYPE.BAT, basechar = 'b', color = ColorHex.GrayDark,
            HD = 1, avg_hp = 10, AC = 0, EV = 0, REF = 0, WIL = 0, FOR = 0, LUK = 0,baseMovSpeed = 0.5f, baseAtkSpeed = 1,
            faction = FacType.PREDATORS,
            parts = PartDataUtils.FLYERPARTS(),
        },
        new monsterentry { mid = MONTYPE.RAT, basechar = 'r', color = ColorHex.BlueDark,
            HD = 1, avg_hp = 10, AC = 0, EV = 0, REF = 0, WIL = 0, FOR = 0, LUK = 0,baseMovSpeed = 1f, baseAtkSpeed = 1,
            faction = FacType.PREDATORS,
            parts = PartDataUtils.QUADRAPED(),
        },

    };

    public static monsterentry GetMonsterEntry(MONTYPE id)
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
    public MONTYPE mid;
    public char basechar;
    public string color;
    // monster flags
    // resists
    public FacType faction;
    public int HD;
    public int avg_hp;
    public short AC;
    public short EV;
    public short REF;
    public short WIL;
    public short FOR;
    public short LUK;
    public float baseMovSpeed;
    public float baseAtkSpeed;
    //public short aggression;
    //item[] inventory
    public PartData[] parts;
    //MUTTYPE[] mutations
    //SPELLTYPE[] spells;
    //itemusetype
    
}