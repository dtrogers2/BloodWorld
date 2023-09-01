using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RndBox_Algo
{
    private List<Room> rooms = new List<Room>();
    public IRegion run(Vector2Int posStart, Vector2Int posEnd, Rng r, MapDrawerIF dm)
    {
        //if (ENTITY.bitHas(dm.map.regionflags, (uint)REGIONFLAGS.CLOSED)) { MapBuilder.addFence(dm.map, (uint)ENV.WALL, (uint)ENV.WALL); }
        //else MapBuilder.addFloor(dm.map);
        // Swap the start and end
        if (posStart.x > posEnd.x)
        {
            int swap = posStart.x;
            posStart.x = posEnd.x;
            posEnd.x = swap;
        }
        if (posStart.y > posEnd.y)
        {
            int swap = posStart.y;
            posStart.y = posEnd.y;
            posEnd.y = swap;
        }
        Vector2Int dim = new Vector2Int(posEnd.x - posStart.x, posEnd.y - posStart.y);
        int area = dim.x * dim.y;

        int ratio = 85;

        int numRoom = 3;//area / ratio;
        for (int i = 0; i < numRoom; i++)
        {
            addBox(dm, dim, r);
        }
        MapBuilder.connectRooms(rooms, r, dm, ENV.FLOOR, ENV.PATH);
        MapBuilder.addExits(dm, r, rooms);
        MapBuilder.adjustWalls(dm.map);
        return dm.map;
    }

    public void addBox(MapDrawerIF dm, Vector2Int dim, Rng r)
    {
        int a = r.rngC(5, 8);
        int b = r.rngC(8, 13 - a);
        bool vert = r.oneIn(2);
        int w = (vert ? a : b);
        int h = (vert ? b : a);
        Vector2Int e = new Vector2Int(r.rng(0, dim.x - w), r.rng(0, dim.y - h));
        Vector2Int f = new Vector2Int(w, h);
        if (ENTITY.bitHas(dm.map.regionflags, (uint)REGIONFLAGS.CLOSED))
        {
            MapBuilder.makeRect(e, f, dm, ENV.FLOOR, ENV.FLOOR);
        } else
        {
            MapBuilder.makeRect(e, f, dm, ENV.WALL, ENV.FLOOR, ENV.FLOOR);
        }
        
        rooms.Add(new Room { pos = e, dim = f, hasWall = !ENTITY.bitHas(dm.map.regionflags, (uint)REGIONFLAGS.CLOSED) });
    }
}
