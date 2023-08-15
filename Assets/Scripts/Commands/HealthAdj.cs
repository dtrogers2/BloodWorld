using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;

public class HealthAdj
{
    public static void adjust(Creature c, int amt, IGame game, Creature src = null)
    {
        if (amt == 0) return;
        if (amt > 0)
        {
            heal(c, amt);
            return;
        }
        if (amt < 0)
        {
            dmg(c, -amt, game, src);
            return;
        }
    }

    public static void heal(Creature c, int amt)
    {
        int limit = c.maxhp - c.hp;
        if (amt > limit) amt = limit;
        c.hp += amt;
    }

    public static void dmg(Creature c, int amt, IGame game, Creature src = null)
    {
        c.hp -= amt;
        bool playerRelated = (c == game.player || (src != null && src == game.player));
        if (c.hp <= 0) mobDies(c, game, playerRelated);
    }

    public static void mobDies(Creature c, IGame game, bool playerRelated)
    {
        string s = $"{c.name} dies";
        if (playerRelated) game.msg(s);
        if (c != game.player)
        {
            game.world.removeEntity(c, game);
        }
        dropItems();
    }

    public static void dropItems()
    {

    }
}
