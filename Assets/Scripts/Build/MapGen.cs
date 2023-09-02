using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
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
        MapBuilder.addFloor(map);
        int num = 50;
        Vector3Int UL = new Vector3Int();
        Vector3Int XT = new Vector3Int();
        for (int n = 0; n < num; n++)
        {
            pick(map, UL, XT, out UL, out XT);
            bool filled = rng.oneIn(3);
            draw(map, UL, XT, filled);
            
        }
        MapBuilder.adjustWalls(map);
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
        int x2 = XT.x - 1;
        int y2 = XT.y - 1;
        List<Vector3Int> seconds = new List<Vector3Int>();
        Vector3Int p = new Vector3Int();
        bool isWater = rng.pct(25);
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

                if ((!edge && (secs || filled)) || (isWater && filled))
                {
                    if (isWater && filled)
                    {
                        map.setCellEntity(Env.get(ENV.WATER), p);

                    } else
                    {
                        map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, p);
                        map.setCellEntity(Env.get(ENV.WALL), p);
                    }

                }
                if (edge && !(isWater && filled)) seconds.Add(p);
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
                map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, edges[ix]);
                map.setCellEntity(Env.get(ENV.FLOOR), edges[ix]);
            }
            
        }
    }

    public static IRegion test(IGame game, Vector3Int regionPos)
    {
        Vector2Int dim = Term.StockDim();
        int seed2 = regionPos.y;
        int seed3 = regionPos.z;
        Rng r = new Rng(game.rng.getSeed() + regionPos.x + (seed2 * 100) + (seed3 * 1000));
        MapGen gen = new MapGen(game, r);
        Region map = new Region(dim, regionPos);
        if (r.oneIn(2)) map.regionflags = ENTITY.bitSet(map.regionflags, (uint)REGIONFLAGS.CLOSED);
        //Add neighor map exits
        if (game.world.hasRegion(regionPos + Vector3Int.down))
        {
            map.exits[(uint)EXITS.NORTH] = game.world.regions[regionPos.x, regionPos.y - 1, regionPos.z].exits[(uint)EXITS.SOUTH];
            if (map.exits[(uint)EXITS.NORTH].Length > 0)
                map.exits[(uint)EXITS.NORTH][0].y = 0;
        }
        if (game.world.hasRegion(regionPos + Vector3Int.right))
        {
            map.exits[(uint)EXITS.EAST] = game.world.regions[regionPos.x + 1, regionPos.y, regionPos.z].exits[(uint)EXITS.WEST];
            if (map.exits[(uint)EXITS.EAST].Length > 0)
                map.exits[(uint)EXITS.EAST][0].x = map.dim.x - 1;
        }
        if (game.world.hasRegion(regionPos + Vector3Int.up))
        {
            map.exits[(uint)EXITS.SOUTH] = game.world.regions[regionPos.x, regionPos.y + 1, regionPos.z].exits[(uint)EXITS.NORTH];
            if (map.exits[(uint)EXITS.SOUTH].Length > 0)
                map.exits[(uint)EXITS.SOUTH][0].y = map.dim.y - 1;
        }
        if (game.world.hasRegion(regionPos + Vector3Int.left))
        {
            map.exits[(uint)EXITS.WEST] = game.world.regions[regionPos.x - 1, regionPos.y, regionPos.z].exits[(uint)EXITS.EAST];
            if (map.exits[(uint)EXITS.WEST].Length > 0)
                map.exits[(uint)EXITS.WEST][0].x = 0;
        }
        BaseMap bm = new BaseMap(dim, map);
        int chance = r.rng(7);
        return new RndBox_Algo().run(Vector2Int.zero, dim, r, bm);
        /*
        switch (chance)
        {
            case 0: return new BrokenColumn_Algo().run(Vector2Int.zero,dim, rng, bm);
            case 1: return new HorzVert_Algo().run(Vector2Int.zero, dim, rng, bm);
            case 2: return new HorzVert_Algo().run(Vector2Int.zero, dim, rng, bm);
            case 3: return gen.loop(map, rng);
            default: return new RndBox_Algo().run(Vector2Int.zero, dim, rng, bm); ;
        }*/
    }



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
