using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRegion
{
    Vector2Int dim { get; }
    public Vector3Int regionPos { get; }
    public TurnQueue queue { get; }
    void moveEntity(IEntity entity, Vector3Int newPos, Vector3Int localSrcPos, Vector3Int localNewPos);
    void addEntity(IEntity entity);
    void removeEntity(IEntity entity, int x, int y);
    void enterMap(IEntity entity, int x, int y);
    Tile tile(int x, int y);
    bool legal(int x, int y);
    bool blocked(int x, int y);
    int level { get; set; }
}

public class Region : IRegion
{
    public Vector2Int dim { get; }
    public Vector3Int regionPos { get; }
    public Tile[,] tiles;
    public int level { get; set; }
    public TurnQueue queue {get;} = new TurnQueue();

    public Region(Vector2Int dim, Vector3Int pos, TermChar floor)
    {
        this.dim = dim;
        this.tiles = allocMap(floor);
        this.regionPos = pos;
    }

    public Tile[,] allocMap(TermChar floor)
    {
        Tile[,] newTiles = new Tile[this.dim.x, this.dim.y];
        for (int y = 0; y < dim.y; y++)
        {
            for (int x = 0; x < dim.x; x++)
            {
                newTiles[x, y] = new Tile(floor, x, y);
            }
        }
        return newTiles;
    }

    public bool blocked(int x, int y)
    {
        if (!legal(x, y)) return true;
        return tile(x, y).blocks();
    }

    public bool blocked(Vector2Int position)
    {
        if (!legal(position)) return true;
        return tile(position).blocks();
    }

    public bool legal(int x, int y)
    {
        return (x >= 0 && x < this.dim.x && y >= 0 && y < this.dim.y);
    }

    public bool legal(Vector2Int position)
    {
        return (position.x >= 0 && position.x < this.dim.x && position.y >= 0 && position.y < this.dim.y);
    }

    public void moveEntity(IEntity entity, Vector3Int newPos, Vector3Int localSrcPos, Vector3Int localNewPos)
    {
        tile(localSrcPos.x, localSrcPos.y).creature = null;
        entity.position = newPos;
        tile(localNewPos.x, localNewPos.y).creature = (Creature) entity;
    }

    public void removeEntity(IEntity entity, int x, int y)
    {
        queue.remove((Creature) entity);
        tile(x, y).creature = null;
    }
    public void addEntity(IEntity entity)
    {
        tile(entity.position.x, entity.position.y).creature = (Creature)entity;
        queue.push((Creature)entity);
    }
    public Tile tile(int x, int y)
    {
        return this.tiles[x, y];
    }

    public Tile tile(Vector2Int position)
    {
        return this.tiles[position.x, position.y];
    }

    public void enterMap(IEntity c, int newX, int newY)
    {
        //Debug.Log("enterMap xy: " + newX + " " + newY);
        //Debug.Log("enterMap c: " + c.position);

        tile(newX, newY).creature = (Creature) c;
        this.queue.push((Creature) c);
    }

    public void resetNodes()
    {
        for (int y = 0; y < dim.y; y++)
        {
            for (int x = 0; x < dim.x; x++)
            {
                tiles[x, y].resetNode();
            }
        }
    }

    public List<Tile> getNeighbors(Vector2Int position)
    {
        List<Tile> retList = new List<Tile>();
        if (!blocked(position + Vector2Int.up)) retList.Add(tile(position + Vector2Int.up));
        if (!blocked(position + Vector2Int.down)) retList.Add(tile(position + Vector2Int.down));
        if (!blocked(position + Vector2Int.left)) retList.Add(tile(position + Vector2Int.left));
        if (!blocked(position + Vector2Int.right)) retList.Add(tile(position + Vector2Int.right));

        if (!blocked(position + Vector2Int.up + Vector2Int.left)) retList.Add(tile(position + Vector2Int.up + Vector2Int.left));
        if (!blocked(position + Vector2Int.up + Vector2Int.right)) retList.Add(tile(position + Vector2Int.up + Vector2Int.right));
        if (!blocked(position + Vector2Int.down + Vector2Int.left)) retList.Add(tile(position + Vector2Int.down + Vector2Int.left));
        if (!blocked(position + Vector2Int.down + Vector2Int.right)) retList.Add(tile(position + Vector2Int.down + Vector2Int.right));
        return retList;
    }

    public Tile getUnblockedGoal(Tile center, Tile start)
    {

        bool found = false;
        int xmin = center.position.x;
        int xmax = center.position.x;
        int ymin = center.position.y;
        int ymax = center.position.y;
        Tile newGoal = center;

        do
        {
            for (int y = ymin; y <= ymax; y++)
            {
                for (int x = xmin; x <= xmax; x++)
                {

                    if (!blocked(x,y))
                    {

                        if (!found)
                        {
                            newGoal = tile(x, y);
                            found = true;
                        }
                        if (found == true)
                        {
                            if ((tile(x, y).distanceBetween(center) <= newGoal.distanceBetween(center)) && tile(x, y).distanceBetween(start) <= newGoal.distanceBetween(start))
                            {
                                newGoal = tile(x, y);
                            }
                        }
                    }
                }
            }
            if (xmin - 1 > 0) xmin--;
            if (xmax + 1 < dim.x - 1) xmax++;
            if (ymin - 1 > 0) ymin--;
            if (ymax + 1 < dim.y - 1) ymax++;
        } while (found == false);
        return newGoal;
    }
}
