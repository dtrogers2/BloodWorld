using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visbility
{
    public static bool canSee(Vector3Int a, Vector3Int b, IGame game, bool onlyEnv = false)
    {
        BresIter i = BresIter.bresIter(a, b);
        for (;!i.done();)
        {
            Vector2Int p = i.next();
            if (game.world.getTile((Vector3Int) p, game, out Tile t))
            {
                if (t.position == a || t.position == b) { continue; }
                if (onlyEnv)
                {
                    if (!t.traversable()) return false;
                } else
                {
                    if (t.blocks()) return false;
                }
               
            }
        }
        return true;
    }
}
