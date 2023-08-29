using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IRegion
{
    Vector2Int dim { get; }
    public Vector3Int regionPos { get; }
    public List<Creature> creatureList { get; }
    public void addEntity(IEntity entity);
    public void removeEntity(IEntity entity);
    public void addEntity(uint entity);
    public void removeEntity(uint entity);
    Tile tile(Vector3Int pos);
    bool legal(Vector3Int pos);
    bool blocked(Vector3Int pos);

    public bool hasCellFlags(CELLFLAG flags, Vector3Int position);
    public uint getCellFlags(Vector3Int position);

    public void setCellFlags(CELLFLAG flags, Vector3Int position);
    public void delCellFlags(CELLFLAG flags, Vector3Int position);

    public uint getCellEntity(Vector3Int position);

    public void setCellEntity(uint id, Vector3Int position);
    public void removeCellEntity(Vector3Int position);
    public uint[,] cells { get; }
    public HashSet<uint> entities { get; }
}

public class Region : IRegion
{
    public Vector2Int dim { get; }
    public Vector3Int regionPos { get; }
    public List<Creature> creatureList { get; } = new List<Creature>();
    public Tile[,] tiles;
    public uint[,] cells { get; }
    public HashSet<uint> entities { get; } = new HashSet<uint>();

    public Region(Vector2Int dim, Vector3Int regionPos, TermChar floor)
    {
        this.dim = dim;
        this.regionPos = regionPos;
        this.tiles = allocMap(floor);
        cells = new uint[dim.x, dim.y];

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


    public bool hasCellFlags(CELLFLAG flags, Vector3Int position)
    {
        return ENTITY.bitHas(cells[position.x % dim.x, position.y % dim.y], (uint)flags);
    }
    public uint getCellFlags(Vector3Int position)
    {
        
        return cells[position.x % dim.x, position.y % dim.y];
    }

    public void setCellFlags(CELLFLAG flags, Vector3Int position)
    {
        cells[position.x % dim.x, position.y % dim.y] = ENTITY.bitSet(cells[position.x % dim.x, position.y % dim.y], (uint)flags);
    }

    public void delCellFlags(CELLFLAG flags, Vector3Int position)
    {
        cells[position.x % dim.x, position.y % dim.y] = ENTITY.bitDel(cells[position.x % dim.x, position.y % dim.y], (uint)flags);
    }

    public uint getCellEntity(Vector3Int position)
    {
        return cells[position.x % dim.x, position.y % dim.y] >> 6;
    }

    public void setCellEntity(uint id, Vector3Int position)
    {
        removeCellEntity(position);
        cells[position.x % dim.x, position.y % dim.y] = ENTITY.bitSet(id << 6, cells[position.x % dim.x, position.y % dim.y]);
    }

    public void removeCellEntity(Vector3Int position)
    {
        uint oldBits = getCellFlags(position);
        uint newBits = 0;
        if (ENTITY.bitHas(oldBits, (uint)CELLFLAG.SEEN)) newBits = ENTITY.bitSet(newBits, (uint) CELLFLAG.SEEN);
        if (ENTITY.bitHas(oldBits, (uint)CELLFLAG.BLOCKED)) newBits = ENTITY.bitSet(newBits, (uint)CELLFLAG.BLOCKED);
        if (ENTITY.bitHas(oldBits, (uint)CELLFLAG.OPAQUE)) newBits = ENTITY.bitSet(newBits, (uint)CELLFLAG.OPAQUE);
        if (ENTITY.bitHas(oldBits, (uint)CELLFLAG.EXPLORE_HORIZON)) newBits = ENTITY.bitSet(newBits, (uint)CELLFLAG.EXPLORE_HORIZON);
        if (ENTITY.bitHas(oldBits, (uint)CELLFLAG.MORE_ITEMS)) newBits = ENTITY.bitSet(newBits, (uint)CELLFLAG.MORE_ITEMS);
        cells[position.x % dim.x, position.y % dim.y] = newBits;
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

    public void addEntity(uint eId)
    {
        entities.Add(eId);
    }

    public void removeEntity(uint eId)
    {
        entities.Remove(eId);
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
