using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class HealthAdj
{
    public static void adjust(uint id, int amt, IGame game, uint srcId = uint.MaxValue)
    {
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

    public static void heal(uint  id, int amt)
    {
        if (ENTITY.has(id, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[id];
            int limit = c.hpMax - c.hp;
            if (amt > limit) amt = limit;
            c.hp += amt;
        }
    }

    public static void dmg(uint id, int amt, IGame game, uint srcId = uint.MaxValue)
    {
        if (ENTITY.has(id, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[id];
            c.hp -= amt;
            bool playerRelated = (id == game.playerId || (srcId != uint.MaxValue && srcId == game.playerId));
            if (c.hp <= 0) mobDies(id, game, playerRelated);
        }

    }

    public static void mobDies(uint id, IGame game, bool playerRelated)
    {
        if (ENTITY.has(id, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[id];
            if (playerRelated)
            {
                string s = $"{c.name} dies";
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
