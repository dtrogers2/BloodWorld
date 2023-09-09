using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class HealthAdj
{
    public static void adjust(uint id, int amt, IGame game, uint srcId = uint.MaxValue)
    {
        if (!ENTITY.has(id, COMPONENT.DEFENSES)) return;
        Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[id];
        if (d.hpMax == 0) initHD(id, game);
        if (amt == 0) return;
        if (amt > 0)
        {
            heal(id, amt);
            return;
        }

        if (amt < 0)
        {
            dmg(id, -amt, game, srcId);
            return;
        }
    }

    public static void initHD(uint id, IGame game)
    {
        string hdString = "1d8";
        if (ENTITY.has(id, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[id];
            Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[id];

            //hdString += $"{c.levels[0]}d4 "; // NONE
            //hdString += $"{c.levels[1]}d8 "; // MONSTER
            //hdString += $"{c.levels[2]}d8 "; // FIGHTER
            //hdString += $"{c.levels[3]}d4 "; // MAGICIAN
            //hdString += $"{c.levels[4]}d6 "; // CLERIC
            //hdString += $"{c.levels[5]}d4 "; // THIEF
            //hdString += $"{c.levels[6]}d6 "; // ELF
            //hdString += $"{c.levels[7]}d8 "; // DWARF
            //hdString += $"{c.levels[8]}d6 "; // HALFLING

            //insert constitution adjustment here or something

            int hpRoll = game.rng.roll(hdString);
            if (hpRoll > d.hpMax)
            {
                d.hpMax = hpRoll;
            }
            d.hp = d.hpMax;
        }
    }

    public static void heal(uint id, int amt)
    {
        Defenses def = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[id];
        int limit = def.hpMax - def.hp;
        if (amt > limit) amt = limit;
         def.hp += amt;
    }

    public static void dmg(uint id, int amt, IGame game, uint srcId = uint.MaxValue)
    {
        Defenses def = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[id];
        def.hp -= amt;
        bool playerRelated = (id == game.playerId || (srcId != uint.MaxValue && srcId == game.playerId));
        if (def.hp <= 0) mobDies(id, game, playerRelated);
    }

    public static void mobDies(uint id, IGame game, bool playerRelated)
    {
        if (ENTITY.has(id, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[id];
            if (playerRelated)
            {
                Msg s = new Msg { color = COLOR.Red, text = $"{c.name}({id}) dies." };
                game.msg(s);
            }
            if (id != game.playerId)
            {
                game.world.removeEntity(id, game);
            }
            ENTITY.unsubscribe(id, COMPONENT.CREATURE);
            dropItems();
        }

    }

    public static void dropItems()
    {

    }
}
