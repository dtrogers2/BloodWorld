using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorzVert_Algo
{
    public IRegion run(Vector2Int posStart,Vector2Int posEnd, Rng r, MapDrawerIF dm)
    {
        if (ENTITY.bitHas(dm.map.regionflags, (uint)REGIONFLAGS.CLOSED)) { MapBuilder.addFence(dm.map, (uint)ENV.WALL, (uint)ENV.FLOOR); }
        else MapBuilder.addFloor(dm.map);
        for (Vector2Int p = posStart; p.x < posEnd.x; p.x += 2)
        {
            for (p.y = 0; p.y < posEnd.y - 1; p.y+=2)
            {
                bool vert = r.oneIn(2);
                Vector2Int q = p + (vert ? Vector2Int.up: Vector2Int.right);
                if (q.y > posEnd.y) q.y = posEnd.y;
                if (q.x > posEnd.x) q.x = posEnd.x;
                dm.map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, (Vector3Int)p);
                dm.map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, (Vector3Int)q);
                dm.setp(q, (uint)ENV.WALL);
                dm.setp(p, (uint)ENV.WALL);
            }
        }

        MapBuilder.addExits(dm, r);
        MapBuilder.adjustWalls(dm.map);
        return dm.map;
    }
}
