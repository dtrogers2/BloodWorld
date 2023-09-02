using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Visbility
{
    public static bool lineTo(Vector3Int a, Vector3Int b, IGame game, bool visionOnly = false)
    {
        BresIter i = BresIter.bresIter(a, b);
        for (;!i.done();)
        {
            Vector2Int p = i.next();
            if (game.world.getCellFlags((Vector3Int) p, game, out uint cellFlags))
            {
                if ((p.x == a.x && p.y == a.y) || (p.x == b.x && p.y == b.y)) continue;
                if (visionOnly && ENTITY.bitHas(cellFlags, (uint)(CELLFLAG.OPAQUE))) return false;
                else if (!visionOnly && ENTITY.bitHas(cellFlags, (uint)(CELLFLAG.OPAQUE | CELLFLAG.BLOCKED | CELLFLAG.CREATURE))) return false;
            } else
            {
                return false;
            }
        }
        return true;
    }
}
