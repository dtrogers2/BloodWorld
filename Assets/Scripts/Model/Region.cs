using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IRegion
{
    Vector2Int dim { get; }
    public Vector3Int regionPos { get; }
    public List<Creature> creatureList { get; }
    public void addEntity(IEntity entity);
    public void removeEntity(IEntity entity);
    Tile tile(Vector3Int pos);
    bool legal(Vector3Int pos);
    bool blocked(Vector3Int pos);
}

public class Region : IRegion
{
    public Vector2Int dim { get; }
    public Vector3Int regionPos { get; }
    public List<Creature> creatureList { get; } = new List<Creature>();
    public Tile[,] tiles;

    public Region(Vector2Int dim, Vector3Int regionPos, TermChar floor)
    {
        this.dim = dim;
        this.regionPos = regionPos;
        this.tiles = allocMap(floor);

    }

    public Tile[,] allocMap(TermChar floor)
    {
        Tile[,] newTiles = new Tile[this.dim.x, this.dim.y];
        for (int y = 0; y < dim.y; y++)
        {
            for (int x = 0; x < dim.x; x++)
            {
                newTiles[x, y] = new Tile(floor, (Term.StockDim().x * regionPos.x) + x, (Term.StockDim().y * regionPos.y) + y);
            }
        }
        return newTiles;
    }
    public bool blocked(Vector3Int position)
    {
        if (!legal(position)) return true;
        return tile(position).blocks();
    }

    public bool traversable(Vector3Int position)
    {
        if (!legal(position)) return true;
        return tile(position).traversable();
    }
    public bool legal(Vector3Int position)
    {
        return (position.x >= 0 && position.x < this.dim.x && position.y >= 0 && position.y < this.dim.y);
    }

    public void removeEntity(IEntity entity)
    {
        creatureList.Remove((Creature) entity);
        tile(entity.position).creature = null;
    }

    public void addEntity(IEntity entity)
    {
        if (!creatureList.Contains((Creature)entity))
        {
            creatureList.Add((Creature)entity);
            tile(entity.position).creature = (Creature) entity;
        }
    }

    public Tile tile(Vector3Int position)
    {
        return this.tiles[position.x % dim.x, position.y % dim.y];
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

}
