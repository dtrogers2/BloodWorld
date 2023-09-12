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
        //if (d.hpMax == 0) initHD(id, game);
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
        if (ENTITY.has(id, COMPONENT.DEFENSES))
        {
            Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[id];
            //insert constitution adjustment here or something
            int hpRoll = game.rng.roll(d.HD);
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
        Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[srcId];
        Creature c1 = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[id];
        //Debug.Log($"({c.name}){srcId}->({c1.name}){id}: {def.hp}");
        bool playerRelated = (id == game.playerId ||  srcId == game.playerId);
        if (def.hp <= 0) {
             mobDies(id, game, playerRelated); 
        }
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
        }

        if (id != game.playerId)
        {
            game.world.removeEntity(id, game);
        }
        ENTITY.unsubscribe(id, COMPONENT.CREATURE);
        dropItems(id, game);
        ENTITY.unsubscribeAll(id);

    }

    public static void dropItems(uint id, IGame game)
    {
        if (!ENTITY.has(id, COMPONENT.INVENTORY)) return;
        Inventory inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[id];
        for (int i = inv.items.Count - 1; i > 0; i--)
        {

            ItemSystem.dropItem(id, inv.items[i], game, out float delay);
        }
    }
}
