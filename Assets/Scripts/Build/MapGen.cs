using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MapGen
{
    public IGame game;
    public Rng rng;
    public MapGen(IGame game, Rng rng)
    {
        this.game = game;
        this.rng = rng;
    }


    public IRegion loop(IRegion map, Rng r)
    {
        for (int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0; x < map.dim.x; x++)
            {
               map.setCellEntity(Env.get(ENV.FLOOR), new Vector3Int(x, y));
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
        adjustWalls(map);
        return map;
    }

    public void adjustWalls(IRegion map)
    {
        for (int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0; x < map.dim.x; x++)
            {
                if (y == 0 || y == map.dim.y || x == 0 || x == map.dim.x) continue;
                uint dirFlags = (uint) DIR.CENTER;
                uint diagFlags = (uint)DIR.ALL ;
                if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x, y)), (uint) CELLFLAG.BLOCKED)) { continue; }

                if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x - 1,    y - 1)), (uint) CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint) DIR.NORTHWEST);
                if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x,        y - 1)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)DIR.NORTH);
                if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x + 1,    y - 1)), (uint)CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint)DIR.NORTHEAST);

                if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x - 1,    y)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)(DIR.WEST));
                if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x + 1,    y)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)(DIR.EAST));

                if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x - 1,    y + 1)), (uint)CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint)DIR.SOUTHWEST);
                if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x,        y + 1)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)DIR.SOUTH);
                if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x + 1,    y + 1)), (uint)CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint)DIR.SOUTHEAST );

                
                switch(dirFlags)
                {
                    case 3: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 5: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 7: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 15: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 23:  map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 9: map.setCellEntity(Env.get(ENV.WALL_NS), new Vector3Int(x, y)); break;
                    case 17: map.setCellEntity(Env.get(ENV.WALL_NS), new Vector3Int(x, y)); break;
                    case 29: map.setCellEntity(Env.get(ENV.WALL_NS), new Vector3Int(x, y)); break;
                    case 27: map.setCellEntity(Env.get(ENV.WALL_NS), new Vector3Int(x, y)); break;
                    case 25: map.setCellEntity(Env.get(ENV.WALL_NS), new Vector3Int(x, y)); break;
                    case 21: map.setCellEntity(Env.get(ENV.WALL_NW), new Vector3Int(x, y)); break;
                    case 13: map.setCellEntity(Env.get(ENV.WALL_NE), new Vector3Int(x, y)); break;
                    case 19: map.setCellEntity(Env.get(ENV.WALL_SW), new Vector3Int(x, y)); break;
                    case 11: map.setCellEntity(Env.get(ENV.WALL_SE), new Vector3Int(x, y)); break;
                    case 31:
                        {
                           
                            switch(diagFlags)
                            {
                                case (uint)DIR.ALL: break;
                                case 479: map.setCellEntity(Env.get(ENV.WALL_SE), new Vector3Int(x, y)); break;
                                case 447: map.setCellEntity(Env.get(ENV.WALL_SW), new Vector3Int(x, y)); break;
                                case 383: map.setCellEntity(Env.get(ENV.WALL_NE), new Vector3Int(x, y)); break;
                                case 255: map.setCellEntity(Env.get(ENV.WALL_NW), new Vector3Int(x, y)); break;
                                default: break;
                            }
                            break;
                        }
                     
                    default: break;
                }
                /*
                bool noDiag = true;
                if (ENTITY.bitHas(dirFlags, (uint)(DIR.NORTH | DIR.SOUTH | DIR.WEST | DIR.EAST))
                    && (ENTITY.bitHas(dirFlags, (uint)DIR.NORTHWEST) || ENTITY.bitHas(dirFlags, (uint)DIR.NORTHEAST)
                    || ENTITY.bitHas(dirFlags, (uint)DIR.SOUTHEAST) || ENTITY.bitHas(dirFlags, (uint)DIR.SOUTHWEST)))
                noDiag = false;*/
               // switch(dirFlags) {
                //    case unchecked((uint)DIR.NORTH | (uint) DIR.SOUTH):
                //    case  unchecked((~((uint)DIR.WEST ^ (uint)DIR.EAST) & ((uint)DIR.NORTH | (uint)DIR.SOUTH)) | ((uint) DIR.NORTH ^ (uint) DIR.SOUTH)): map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                //    default: break;
                //}


                /*
                if (ENTITY.bitHas(dirFlags, (uint) (DIR.NORTH | DIR.SOUTH)) && !ENTITY.bitHas(dirFlags, (uint)(DIR.WEST | DIR.EAST)))
                {
                    map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y));
                    continue;
                }

                if (ENTITY.bitHas(dirFlags, (uint)(DIR.WEST | DIR.EAST)) && !ENTITY.bitHas(dirFlags, (uint)(DIR.NORTH | DIR.SOUTH)))
                {
                    map.setCellEntity(Env.get(ENV.WALL_NS), new Vector3Int(x, y));
                    continue;
                }

                if (ENTITY.bitHas(dirFlags, (uint)(DIR.NORTH | DIR.WEST)) && !ENTITY.bitHas(dirFlags, (uint)(DIR.EAST | DIR.SOUTH)))
                {
                    map.setCellEntity(Env.get(ENV.WALL_SE), new Vector3Int(x, y));
                    continue;
                }

                if (ENTITY.bitHas(dirFlags, (uint)(DIR.NORTH | DIR.EAST)) && !ENTITY.bitHas(dirFlags, (uint)(DIR.WEST | DIR.SOUTH)))
                {
                    map.setCellEntity(Env.get(ENV.WALL_SW), new Vector3Int(x, y));
                    continue;
                }

                if (ENTITY.bitHas(dirFlags, (uint)(DIR.SOUTH | DIR.WEST)) && !ENTITY.bitHas(dirFlags, (uint)(DIR.NORTH | DIR.EAST)))
                {
                    map.setCellEntity(Env.get(ENV.WALL_NE), new Vector3Int(x, y));
                    continue;
                }

                if (ENTITY.bitHas(dirFlags, (uint)(DIR.SOUTH | DIR.EAST)) && !ENTITY.bitHas(dirFlags, (uint)(DIR.NORTH | DIR.WEST)))
                {
                    map.setCellEntity(Env.get(ENV.WALL_NW), new Vector3Int(x, y));
                    continue;
                }*/

            }
        }
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
                //map.setCellEntity(game.build.floorId, p);
                map.setCellEntity(Env.get(ENV.FLOOR), p);
                map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, p);

                if (!edge && (secs || filled))
                {
                    map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, p);
                    map.setCellEntity(Env.get(ENV.WALL), p);
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
                //map.removeCellEntity(edges[ix]);
                map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, edges[ix]);
                map.setCellEntity(Env.get(ENV.FLOOR), edges[ix]);
            }
            
        }
    }

    public static IRegion test(IGame game, Vector3Int regionPos, Rng rng)
    {
        Vector2Int dim = Term.StockDim();
        MapGen gen = new MapGen(game, rng);
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

[Flags]
public enum DIR
{
    NONE = 0,
    CENTER = 1, // 1
    NORTH = 1 << 1, // 2
    SOUTH = 1 << 2, // 4
    WEST = 1 << 3, // 8
    EAST = 1 << 4, // 16
    NORTHWEST = 1 << 5, // 32
    NORTHEAST = 1 << 6, // 64
    SOUTHWEST = 1 << 7, // 128
    SOUTHEAST = 1 << 8, // 256
    ALL = 511
} 
