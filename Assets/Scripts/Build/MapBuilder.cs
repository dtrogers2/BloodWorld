using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.XR;
using static UnityEditor.PlayerSettings;

public class MapBuilder
{
    public static void addFence(IRegion map, uint wall = (uint) ENV.WALL, uint fill = (uint) ENV.FLOOR)
    {
        Vector2Int p = Vector2Int.zero;
        for (p.y = 0; p.y < map.dim.y; p.y++)
        {
            for (p.x = 0; p.x < map.dim.x; p.x++)
            {
                bool edge = (p.x == 0 || p.x == map.dim.x - 1 || p.y == 0 || p.y == map.dim.y - 1);
                uint env = edge ? wall : fill;
                if (env == (uint) ENV.EMPTY) continue;
                map.setCellEntity(env, (Vector3Int)p);
                if (env == (uint) ENV.WALL) map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, (Vector3Int)p);
                else map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE,(Vector3Int) p);
            }
        }
    }

    public static bool roomsIntersect(Room a, Room b)
    {
        return !( a.getP().x > b.getP().x + b.getD().x || a.getP().x + a.getD().x < b.getP().x || a.getP().y > b.getP().y + b.getD().y || a.getP().y + a.getD().y < b.getP().y);
    }
    public static void addExits(MapDrawerIF dm, Rng r)
    {
        // Make exits if the map has no exits
        if (dm.map.exits[(uint)EXITS.NORTH].Length == 0)
        {
            int x = r.rng(dm.map.dim.x);
            dm.map.exits[(uint)EXITS.NORTH] = new Vector3Int[] { new Vector3Int(x, 0) };
        }
        if (dm.map.exits[(uint)EXITS.EAST].Length == 0)
        {
            int y = r.rng(dm.map.dim.y);
            dm.map.exits[(uint)EXITS.EAST] = new Vector3Int[] { new Vector3Int(dm.map.dim.x - 1, y) };
        }
        if (dm.map.exits[(uint)EXITS.SOUTH].Length == 0)
        {
            int x = r.rng(dm.map.dim.x);
            dm.map.exits[(uint)EXITS.SOUTH] = new Vector3Int[] { new Vector3Int(x, dm.map.dim.y - 1) };
        }
        if (dm.map.exits[(uint)EXITS.WEST].Length == 0)
        {
            int y = r.rng(dm.map.dim.y);
            dm.map.exits[(uint)EXITS.WEST] = new Vector3Int[] { new Vector3Int(0, y) };
        }

        makePath(new Vector2Int(r.rng(dm.map.dim.x), r.rng(dm.map.dim.y)), (Vector2Int)dm.map.exits[(uint)EXITS.NORTH][0], dm, ENV.FLOOR, ENV.EMPTY);
        makePath(new Vector2Int(r.rng(dm.map.dim.x), r.rng(dm.map.dim.y)), (Vector2Int)dm.map.exits[(uint)EXITS.EAST][0], dm, ENV.FLOOR, ENV.EMPTY);
        makePath(new Vector2Int(r.rng(dm.map.dim.x), r.rng(dm.map.dim.y)), (Vector2Int)dm.map.exits[(uint)EXITS.SOUTH][0], dm, ENV.FLOOR, ENV.EMPTY);
        makePath(new Vector2Int(r.rng(dm.map.dim.x), r.rng(dm.map.dim.y)), (Vector2Int)dm.map.exits[(uint)EXITS.WEST][0], dm, ENV.FLOOR, ENV.EMPTY);
    }

    public static void addExits(MapDrawerIF dm, Rng r, List<Room> rooms)
    {
        // Make exits if the map has no exits
        if (dm.map.exits[(uint)EXITS.NORTH].Length == 0)
        {
            int x = r.rng(dm.map.dim.x);
            dm.map.exits[(uint)EXITS.NORTH] = new Vector3Int[] { new Vector3Int(x, 0) };
        }
        if (dm.map.exits[(uint)EXITS.EAST].Length == 0)
        {
            int y = r.rng(dm.map.dim.y);
            dm.map.exits[(uint)EXITS.EAST] = new Vector3Int[] { new Vector3Int(dm.map.dim.x - 1, y) };
        }
        if (dm.map.exits[(uint)EXITS.SOUTH].Length == 0)
        {
            int x = r.rng(dm.map.dim.x);
            dm.map.exits[(uint)EXITS.SOUTH] = new Vector3Int[] { new Vector3Int(x, dm.map.dim.y - 1) };
        }
        if (dm.map.exits[(uint)EXITS.WEST].Length == 0)
        {
            int y = r.rng(dm.map.dim.y);
            dm.map.exits[(uint)EXITS.WEST] = new Vector3Int[] { new Vector3Int(0, y) };
        }
        Room nearest = nearestRoom(rooms, (Vector2Int)dm.map.exits[(uint)EXITS.NORTH][0]);
        if (dm.map.regionPos.y != 0)
        {
           
            //List<Vector2Int> edges = rectEdges(nearest.pos, nearest.dim);
            //Vector2Int edge = edges[r.rng(edges.Count)];
            makePath(nearest.center, (Vector2Int)dm.map.exits[(uint)EXITS.NORTH][0], dm, ENV.FLOOR, ENV.EMPTY);
        }

        if (dm.map.regionPos.x != World.worldDim.x - 1)
        {
            nearest = nearestRoom(rooms, (Vector2Int)dm.map.exits[(uint)EXITS.EAST][0]);
            //edges = rectEdges(nearest.pos, nearest.dim);
            //edge = edges[r.rng(edges.Count)];
            makePath(nearest.center, (Vector2Int)dm.map.exits[(uint)EXITS.EAST][0], dm, ENV.FLOOR, ENV.EMPTY);
        }

        if (dm.map.regionPos.y != World.worldDim.y - 1)
        {
            nearest = nearestRoom(rooms, (Vector2Int)dm.map.exits[(uint)EXITS.SOUTH][0]);
            //edges = rectEdges(nearest.pos, nearest.dim);
            //edge = edges[r.rng(edges.Count)];
            makePath(nearest.center, (Vector2Int)dm.map.exits[(uint)EXITS.SOUTH][0], dm, ENV.FLOOR, ENV.EMPTY);
        }

        if (dm.map.regionPos.x != 0)
        {
            nearest = nearestRoom(rooms, (Vector2Int)dm.map.exits[(uint)EXITS.WEST][0]);
            //edges = rectEdges(nearest.pos, nearest.dim);
            //edge = edges[r.rng(edges.Count)];
            makePath(nearest.center, (Vector2Int)dm.map.exits[(uint)EXITS.WEST][0], dm, ENV.FLOOR, ENV.EMPTY);
        }


    }

    public static void addFloor(IRegion map, ENV env = ENV.FLOOR)
    {
        for (int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0; x < map.dim.x; x++)
            {
                map.setCellEntity(Env.get(env), new Vector3Int(x, y));
                map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, new Vector3Int(x, y));
            }
        }
    }

    public static void makePath(Vector2Int src, Vector2Int tgt, MapDrawerIF md, ENV env = ENV.FLOOR, ENV hard = ENV.WALL)
    {
        BresIter i = BresIter.bresIter((Vector3Int) src, (Vector3Int) tgt);
        for (; !i.done();)
        {
            Vector2Int p = i.next();
            md.carve(p, env, hard);
        }
    }

    public static void makeRect(Vector2Int src, Vector2Int dim, MapDrawerIF md, ENV env = ENV.FLOOR, ENV hard = ENV.WALL)
    {
        for (int y = 0; y <= dim.y; y++)
        {
            for (int x = 0; x <= dim.x; x++)
            {
                md.carve(new Vector2Int(x + src.x, y + src.y), env, hard);
            }
        }
    }


    public static void makeRect(Vector2Int src, Vector2Int dim, MapDrawerIF md, ENV wall = ENV.WALL, ENV floor = ENV.FLOOR, ENV hard = ENV.FLOOR)
    {
        for (int y = 0; y <= dim.y; y++)
        {
            for (int x = 0; x <= dim.x; x++)
            {
                bool edge = (x == 0 || x == dim.x || y == 0 || y == dim.y);
                md.carve(new Vector2Int(x + src.x, y + src.y), (edge ? wall : floor), (edge ? floor : hard));
            }
        }
    }

    public static void connectRooms(List<Room> rooms, Rng r, MapDrawerIF md, ENV connector = ENV.FLOOR, ENV hard = ENV.FLOOR)
    {
        List<Room> connected = new List<Room>();
        // Iterate through rooms, if any overlap add them to rooms;
        foreach (Room room in rooms)
        {
            foreach (Room other in rooms)
            {
                if (connected.Contains(other)) continue;
                if (room == other) continue;
                if (roomsIntersect(room, other))
                {
                    connected.Add(room);
                    connected.Add(other);
                    break;
                }
            }
        }

        foreach (Room room in rooms)
        {
            if (connected.Contains(room)) continue;
            Room nearest = nearestRoom(rooms, room.center, room);
            //Connect the room and the nearest
            connectCenter(room, nearest, md, connector, hard);
            //Add them both to the connected list
            connected.Add(room);
            connected.Add(nearest);
        }
    }

    public static void connectCenter(Room a, Room b, MapDrawerIF md, ENV connector = ENV.FLOOR, ENV hard = ENV.FLOOR)
    {
        
        Vector2Int aC = a.center;
        Vector2Int bC = b.center;
        makePath(aC, bC, md, connector, hard);
    }

    public static void connectEdge(Room a, Room b, MapDrawerIF md, Rng r, ENV connector = ENV.FLOOR, ENV hard = ENV.FLOOR)
    {
        List<Vector2Int> roomEdges = rectEdges(a.pos, a.dim);
        List<Vector2Int> nearestEdges = rectEdges(b.pos, b.dim);
        Vector2Int edge = roomEdges[r.rng(roomEdges.Count)];
        Vector2Int nearestEdge = nearestEdges[r.rng(nearestEdges.Count)];
        /*
        if (Vector2Int.Distance(Vector2Int.zero, edge) > Vector2Int.Distance(Vector2Int.zero, nearestEdge))
        {
            Vector2Int swap = edge;
            edge = nearestEdge;
            nearestEdge = swap;
        }*/

        makePath(edge, nearestEdge, md, connector, hard);
    }

    public static Room nearestRoom(List<Room> rooms, Vector2Int pos, Room room = null)
    {
        Room nearest = null;
        foreach (Room other in rooms) // Find nearest room
        {
            if (room != null) if (other == room) continue;
            if (nearest == null)
            {
                nearest = other;
                continue;
            }
            if (Vector2Int.Distance(pos, other.center) < Vector2Int.Distance(pos, nearest.center))
            {
                nearest = other;
            }
        }
        return nearest;

    }

    public static List<Vector2Int> rectEdges(Vector2Int pos, Vector2Int dim)
    {
        List<Vector2Int> edges = new List<Vector2Int>();
        for (int y = 0; y <= dim.y; y++)
        {
            for (int x = 0; x <= dim.x; x++)
            {
                bool edge = (x == 0 || x == dim.x || y == 0 || y == dim.y);
                if (edge) edges.Add(new Vector2Int(pos.x + x, pos.y + y));
            }
        }
        return edges;
    }

    public static void adjustWalls(IRegion map)
    {
        for (int y = 0; y < map.dim.y; y++)
        {
            for (int x = 0; x < map.dim.x; x++)
            {
                uint dirFlags = (uint)DIR.CENTER;
                uint diagFlags = (uint)DIR.ALL;
                if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x, y)), (uint)CELLFLAG.BLOCKED)) { continue; }
                if (map.legal(new Vector3Int(x - 1, y - 1)))
                    if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x - 1, y - 1)), (uint)CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint)DIR.NORTHWEST);
                if (map.legal(new Vector3Int(x, y - 1)))
                    if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x, y - 1)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)DIR.NORTH);
                if (map.legal(new Vector3Int(x + 1, y - 1)))
                    if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x + 1, y - 1)), (uint)CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint)DIR.NORTHEAST);
                if (map.legal(new Vector3Int(x - 1, y)))
                    if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x - 1, y)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)(DIR.WEST));
                if (map.legal(new Vector3Int(x + 1, y)))
                    if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x + 1, y)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)(DIR.EAST));
                if (map.legal(new Vector3Int(x - 1, y + 1)))
                    if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x - 1, y + 1)), (uint)CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint)DIR.SOUTHWEST);
                if (map.legal(new Vector3Int(x, y + 1)))
                    if (ENTITY.bitHas(map.getCellFlags(new Vector3Int(x, y + 1)), (uint)CELLFLAG.BLOCKED)) dirFlags = ENTITY.bitSet(dirFlags, (uint)DIR.SOUTH);
                if (map.legal(new Vector3Int(x + 1, y + 1)))
                    if (!ENTITY.bitHas(map.getCellFlags(new Vector3Int(x + 1, y + 1)), (uint)CELLFLAG.BLOCKED)) diagFlags = ENTITY.bitDel(diagFlags, (uint)DIR.SOUTHEAST);


                switch (dirFlags)
                {
                    case 3: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 5: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 7: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 15: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
                    case 23: map.setCellEntity(Env.get(ENV.WALL_WE), new Vector3Int(x, y)); break;
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

                            switch (diagFlags)
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

            }
        }
    }
}

public class Room
{
    public Vector2Int pos;
    public Vector2Int dim;
    public bool hasWall;
    public Vector2Int center {get => new Vector2Int(pos.x + dim.x / 2, pos.y + dim.y / 2); private set => throw new System.Exception("Can't set room center!"); }

    public Vector2Int getP()
    {
        return new Vector2Int(pos.x + (hasWall? 1 : 0), pos.y + (hasWall ? 1 : 0));
    }
    public Vector2Int getD()
    {
        return new Vector2Int(dim.x - (hasWall ? 1 : 0), dim.y - (hasWall ? 1 : 0));
    }
}