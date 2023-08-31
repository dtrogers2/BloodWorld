using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class MonData
{
    public static monsterentry[] entries = new monsterentry[]
    {
        new monsterentry { mid = MONTYPE.ERROR, data = new object[2] {new Glyph { c = 'E', color = ColorHex.Red },
            new Creature { name = "Error", actionPoints = 0f, attackRate = 1f, moveRate = 1f, hp = 1, hpMax = 1 } }
            , components = new COMPONENT[2] {COMPONENT.GLYPH, COMPONENT.CREATURE}
        },
        new monsterentry { mid = MONTYPE.HUMAN, data = new object[2] {new Glyph { c = 'h', color = ColorHex.White }, new Creature { name = "Human", actionPoints = 0f, attackRate = 1f, moveRate = 1f, hp = 3, hpMax = 3 } }, components = new COMPONENT[2] {COMPONENT.GLYPH, COMPONENT.CREATURE}
        },
        new monsterentry { mid = MONTYPE.BAT, data = new object[2] {new Glyph { c = 'b', color = ColorHex.GrayDark }, new Creature { name = "Bat", actionPoints = 0f, attackRate = 1f, moveRate = 0.5f, hp = 2, hpMax = 2 } }, components = new COMPONENT[2] {COMPONENT.GLYPH, COMPONENT.CREATURE}
        },
        new monsterentry { mid = MONTYPE.RAT, data = new object[2] {new Glyph { c = 'r', color = ColorHex.BlueDark }, new Creature { name = "Rat", actionPoints = 0f, attackRate = 1f, moveRate = 1f, hp = 1, hpMax = 1 } }, components = new COMPONENT[2] {COMPONENT.GLYPH, COMPONENT.CREATURE}
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
    public object[] data;
    public COMPONENT[] components;
}