using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;


public class MonData
{
    public static monsterentry[] entries = new monsterentry[]
    {
        new monsterentry { mid = MONTYPE.ERROR, data = new object[] {
            new Glyph { c = 'E', color = ColorHex.Red },
            new Creature { name = "Error", AP = 0f, moveSpeed = 0, vision = 5} }
            , components = new COMPONENT[2] {COMPONENT.GLYPH, COMPONENT.CREATURE}
        },
        new monsterentry { mid = MONTYPE.HUMAN, data = new object[] {
            new Glyph { c = '@', color = ColorHex.White },
            new Creature { name = "Human", AP = 0f, moveSpeed = 30, vision = 5, classes = CLASS.NONE, levels = new int[] {1,0,0,0,0,0,0,0,0} },
            new Defenses {},
            new Attacks { baseAtkRate = 1f, atkUsed = 0, attacks = new Attack[1] { new Attack{name = "strike", dmgDice = "1d3", atkRate = 1f} } }
            },
            components = new COMPONENT[] {COMPONENT.GLYPH, COMPONENT.CREATURE, COMPONENT.DEFENSES, COMPONENT.ATTACK}
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