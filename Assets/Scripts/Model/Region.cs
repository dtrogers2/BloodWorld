using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public interface IRegion
{
    Vector2Int dim { get; }
    public Vector3Int regionPos { get; }
    public uint regionflags { get; set; }
    public Vector3Int[][] exits { get; set; }
    public void addEntity(uint entity);
    public void removeEntity(uint entity);
    bool legal(Vector3Int pos);

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
    public uint[,] cells { get; }
    public HashSet<uint> entities { get; } = new HashSet<uint>();
    public uint regionflags { get; set; }
    public Vector3Int[][] exits { get; set; } = new[] { new Vector3Int[0], new Vector3Int[0] , new Vector3Int[0] , new Vector3Int[0], new Vector3Int[0], new Vector3Int[0] };

    public Region(Vector2Int dim, Vector3Int regionPos)
    {
        this.dim = dim;
        this.regionPos = regionPos;
        cells = new uint[dim.x, dim.y];
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
        return cells[position.x % dim.x, position.y % dim.y] >> Enum.GetNames(typeof(CELLFLAG)).Length;
    }

    public void setCellEntity(uint id, Vector3Int position)
    {
        removeCellEntity(position);
        cells[position.x % dim.x, position.y % dim.y] = ENTITY.bitSet(id << Enum.GetNames(typeof(CELLFLAG)).Length, cells[position.x % dim.x, position.y % dim.y]);
    }

    public void removeCellEntity(Vector3Int position)
    {
        uint oldBits = getCellFlags(position);
        uint newBits = 0;
        CELLFLAG[] cellflags = (CELLFLAG[])Enum.GetValues(typeof(CELLFLAG));
        for (int i = 1; i < cellflags.Length; i++)
        {
            if (ENTITY.bitHas(oldBits, (uint)cellflags[i])) newBits = ENTITY.bitSet(newBits, (uint) cellflags[i]);

        }
        cells[position.x % dim.x, position.y % dim.y] = newBits;
    }

    public bool legal(Vector3Int position)
    {
        return (position.x >= 0 && position.x < this.dim.x && position.y >= 0 && position.y < this.dim.y);
    }

    public void addEntity(uint eId)
    {
        entities.Add(eId);

        if (ENTITY.has(eId, COMPONENT.POSITION))
        {
            Position p = (Position)ComponentManager.get(COMPONENT.POSITION).data[eId];
            Vector3Int pos = new Vector3Int(p.x, p.y, p.z);
            uint otherId = getCellEntity(pos);
            if (otherId != eId && otherId != 0)
            {
                CellStack o = new CellStack { entity = otherId };
                ENTITY.subscribe(eId, o);
            }
            setCellEntity(eId, pos);

            if (ENTITY.has(eId, COMPONENT.CREATURE))
            {
                setCellFlags(CELLFLAG.CREATURE, pos);
            }
        }
    }

    public void removeEntity(uint eId)
    {
        entities.Remove(eId);
        // If entity has a position, remove its flag from the cell
        if (ENTITY.has(eId, COMPONENT.POSITION))
        {
            Position p = (Position) ComponentManager.get(COMPONENT.POSITION).data[eId];
            Vector3Int pos = new Vector3Int(p.x, p.y, p.z);
            if (getCellEntity(pos) == eId)
            {
                removeCellEntity(new Vector3Int(p.x, p.y, p.z));
                if (ENTITY.has(eId, COMPONENT.CELLSTACK)) // If an entity was over something, add it back to the cell
                {
                    CellStack o = (CellStack) ComponentManager.get(COMPONENT.CELLSTACK).data[eId];
                    setCellEntity(o.entity, pos);
                    ENTITY.unsubscribe(eId, COMPONENT.CELLSTACK);
                }
            } else if (ENTITY.has(eId, COMPONENT.CELLSTACK)) // If it is not the cell entity and has a cellstack, reassign parent to point to child and remove
            {
                List<uint> stack = new List<uint>();
                stack.Add(getCellEntity(pos));
                CellStack cMain = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[getCellEntity(pos)];
                uint subC = cMain.entity;
                stack.Add(subC);
                bool hasStack = true;
                while (hasStack)
                {
                    if (ENTITY.has(subC, COMPONENT.CELLSTACK))
                    {
                        CellStack c2 = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[subC];
                        subC = c2.entity;
                        stack.Add(subC);
                    }
                    else
                    {
                        hasStack = false;
                    }
                }

                int indexMe = stack.IndexOf(eId);
                int indexParent = indexMe - 1;
                int indexChild = indexMe + 1;
                uint parent = stack.ElementAt(indexParent);
                uint child = stack.ElementAt(indexChild); 
                CellStack cParent = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[parent];
                cParent.entity = child;

                ENTITY.unsubscribe(eId, COMPONENT.CELLSTACK);
            }

            if (ENTITY.has(eId,COMPONENT.CREATURE))
            {
                delCellFlags(CELLFLAG.CREATURE, pos);
            }
        }
    }

}
[Flags]
public enum REGIONFLAGS: uint
{
    NONE = 0,
    OPEN = 1,
    CLOSED = 1 << 1,
    SURFACE = 1 << 2,
    DUNGEON = 1 << 3,
    BADLANDS = 1 << 4,
    HILLS = 1 << 4,
    MOUNTAINS = 1 << 4,
    CITY = 1 << 5,
    GRASSLANDS = 1<< 6,
    DESERT = 1 << 7,
    FOREST = 1 << 8,
    JUNGLE = 1 << 9,
    LAKE = 1 << 10,
    RIVER = 1 << 10,
    OCEAN = 1 << 11,
    SETTLED = 1 << 12,
    SWAMP = 1 << 13,
    LAIR = 1 << 14,
    STRONGHOLD = 1 << 15
}

public enum EXITS: int
{
    NORTH = 1,
    EAST = 2,
    SOUTH = 3,
    WEST = 4,
    UP = 5,
    DOWN = 6,
}