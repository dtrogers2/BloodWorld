using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGen
{
    public IGame game;
    public Rng rng;
    public Wall wall;
    public MapGen(IGame game, Rng rng, Wall wall)
    {
        this.game = game;
        this.rng = rng;
        this.wall = wall;
    }


    public IRegion loop(IRegion map, Rng r)
    {
        for (int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0; x < map.dim.x; x++)
            {
               map.setCellEntity(game.build.floorId, new Vector3Int(x, y));
            }
        }
        int num = 50;
        Vector3Int UL = new Vector3Int();
        Vector3Int XT = new Vector3Int();
        for (int n = 0; n < num; n++)
        {
            pick(map, UL, XT, out UL, out XT);
            bool filled = rng.oneIn(3);
            draw(map, UL, XT, filled);
            
        }
        return map;
    }

    public void pick(IRegion map, Vector3Int UL, Vector3Int XT, out Vector3Int UL2, out Vector3Int XT2)
    {
        Vector2Int dim = Term.StockDim();
        XT.y = rng.rngC(2, 8);
        XT.x = rng.rngC(4, 12);
        if (rng.oneIn(2))
        {
            int s = XT.x;
            XT.x = XT.y;
            XT.y = s;
        }

        UL.x = rng.rng(dim.x - XT.x);
        UL.y = rng.rng(dim.y - XT.y);
        XT2 = XT;
        UL2 = UL;
    }

    public void draw(IRegion map, Vector3Int UL, Vector3Int XT, bool filled)
    {
        Wall center = (filled) ? wall : null;
        int x2 = XT.x - 1;
        int y2 = XT.y - 1;
        List<Vector3Int> seconds = new List<Vector3Int>();
        Vector3Int p = new Vector3Int();
        for (int y = 0; y <= XT.y; y++)
        {
            p.y = y + UL.y;
            for (int x = 0; x <= XT.x; x++)
            {
                p.x = x + UL.x;
                bool edge = (x == 0 || y == 0 || x == XT.x || y == XT.y);
                bool secs = (x == 1 || y == 1 || x == x2 || y == y2);
                Wall w = edge ? null : (secs) ? wall : center;

                map.tile(p).wall = w;
                map.setCellEntity(game.build.floorId, p);
                map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, p);

                if (!edge && (secs || filled))
                {
                    map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, p);
                    map.setCellEntity(game.build.wallId, p);
                    
                }
                if (edge) seconds.Add(p);
            }
        }
        if (!filled) makeDoors(map, seconds);
    }

    public void makeDoors(IRegion map, List<Vector3Int> edges)
    {
        for (int i = rng.rng(1, 3); i >= 0; i--)
        {
            int ix = rng.rng(0, edges.Count);
            if (ix < edges.Count)
            {
                map.tile(edges[ix]).wall = null;
                //map.removeCellEntity(edges[ix]);
                map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, edges[ix]);
                map.setCellEntity(game.build.floorId, edges[ix]);
            }
            
        }
    }

    public static IRegion test(IGame game, Vector3Int regionPos, Rng rng, Wall wall)
    {
        Vector2Int dim = Term.StockDim();
        MapGen gen = new MapGen(game, rng, wall);
        Region map = new Region(dim, regionPos, new TermChar { background = ColorHex.Black, c = '.', foreground = ColorHex.GrayDark });
        return gen.loop(map, rng);
    }

    public bool roomsIntersect(Room a, Room b)
    {
        return (a.y + a.height > b.y || b.y + b.height > a.y || a.x + a.width > b.x || b.x + b.width > a.x);
    }

}

public struct Room
{
    public int x;
    public int y;
    public int width;
    public int height;
    public bool isHall;
    public bool hasHall;
}
