using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrokenColumn_Algo
{
   public IRegion run(Vector2Int posStart, Vector2Int posEnd, Rng r, MapDrawerIF dm)
    {
        if (ENTITY.bitHas(dm.map.regionflags, (uint) REGIONFLAGS.CLOSED)) { MapBuilder.addFence(dm.map, (uint)ENV.WALL, (uint)ENV.FLOOR); }
        else MapBuilder.addFloor(dm.map);
        for (Vector2Int p = new Vector2Int(posStart.x + 2, posStart.y); p.x < posEnd.x; p.x+=2)
        {
            int a = r.rng(1, posStart.y + posEnd.y - 1);
            int b = r.rng(1, posStart.y + posEnd.y - 1);
            for(p.y = 0; p.y < posEnd.y ; p.y++)
            {
                if (p.y == a || p.y == b) {
                    dm.map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, (Vector3Int) p);
                    dm.setp(p, (uint)ENV.FLOOR);
                } else {
                    dm.map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, (Vector3Int) p);
                    dm.setp(p, (uint) ENV.WALL);
                }
            }
        }
        MapBuilder.addExits(dm, r);
        MapBuilder.adjustWalls(dm.map);
        return dm.map;
    }
}
