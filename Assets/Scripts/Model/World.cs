using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.PlayerSettings;

public class World
{
    Vector3Int worldDim;
    IRegion[,,] regions;

    public World(Vector3Int worldDim)
    {
        this.worldDim = worldDim;
        regions = new IRegion[worldDim.x, worldDim.y, worldDim.z];
    }

    public bool getRegion(Vector3Int worldPos, IGame game, out IRegion r)
    {
        r = null;
        if (!(worldPos.x >= 0 && worldPos.x < this.worldDim.x * Term.StockDim().x 
            && worldPos.y >= 0 && worldPos.y < this.worldDim.y * Term.StockDim().y 
            && worldPos.z >= 0 && worldPos.z < worldDim.z)) return false;
        Vector3Int rP = regionPosition(worldPos);
        if (!hasRegion(rP))
        {
            IRegion map = game.build.makeLevel(game, rP);
            add(map, rP);
        }
        r = regions[rP.x, rP.y, rP.z];
        return true;
    }
    // Gets region from a regionPosition
    public IRegion getRegion2(Vector3Int regionPos, IGame game)
    {
        if (!hasRegion(regionPos))
        {
            IRegion map = game.build.makeLevel(game, regionPos);
            add(map, regionPos);
        }
        return this.regions[regionPos.x, regionPos.y, regionPos.z];
    }


    public Stack<IRegion> getRegions(Vector3Int position, int scale, IGame game)
    {
        Stack<IRegion> rS = new Stack<IRegion>();
        Vector3Int rP = regionPosition(position);

        for (int y = rP.y - scale; y <= rP.y + scale; y++)
        {
            for (int x = rP.x - scale; x <= rP.x + scale; x++)
            {
                if (x >= 0 && y >= 0 && x < worldDim.x && y < worldDim.y && rP.z >= 0 && rP.z < worldDim.z)
                {
                    rS.Push(getRegion2(new Vector3Int(x, y), game));
                }
            }
        }

        return rS;
    }




    public bool getCellFlags(Vector3Int worldPos, IGame game, out uint cellBits)
    {
        cellBits = 0;
        Vector2Int stockTerm = Term.StockDim();
        if (worldPos.x >= 0 && worldPos.x < worldDim.x * stockTerm.x && worldPos.y >= 0 && worldPos.y < worldDim.y * stockTerm.y && worldPos.z >= 0 && worldPos.z < worldDim.z)
        {
            if (getRegion(worldPos, game, out IRegion r))
            {

                cellBits = r.getCellFlags(worldPos);
                if (cellBits == 0) { return false; }

                return true;
            }
        }
        return false;
    }
    public bool getCellEntity(Vector3Int worldPos, IGame game, out uint cellBits)
    {
        cellBits = 0;
        Vector2Int stockTerm = Term.StockDim();
        if (worldPos.x >= 0 && worldPos.x < worldDim.x * stockTerm.x && worldPos.y >= 0 && worldPos.y < worldDim.y * stockTerm.y && worldPos.z >= 0 && worldPos.z < worldDim.z)
        {
            if (getRegion(worldPos, game, out IRegion r))
            {
                
                cellBits = r.getCellEntity(worldPos);
                if (cellBits == 0) { return false; }
                
                return true;
            }
        }
        return false;
    }

    public void add(IRegion region, Vector3Int pos)
    {
        regions[pos.x, pos.y, pos.z] = region;
    }

    public bool hasRegion(Vector3Int pos)
    {
        if (pos.x >= 0 && pos.x < this.worldDim.x && pos.y >= 0 && pos.y < this.worldDim.y && pos.z >= 0)
        {
            return (regions[pos.x, pos.y, pos.z] != null);
        }
        return false;
    }


    public bool moveEntity(uint c, Vector3Int dest, IGame game)
    {
        if (getCellFlags(dest, game, out uint cell))
        {
            if (!ENTITY.bitHas(cell, (uint) (CELLFLAG.BLOCKED | CELLFLAG.CREATURE)))
            {
                if (removeEntity(c, game))
                {
                    ComponentManager.get(COMPONENT.POSITION).data[c] = new Position { x = dest.x, y = dest.y, z = dest.z };
                    if (addEntity(c, game))
                    {
                        return true;
                    }
                    else
                    {
                        throw new System.Exception("Entity was removed but not added in moveEntity()!!!");
                    }
                }
                return false;
            }
        }
        return false;
    }


    public bool removeEntity(uint c, IGame game)
    {
        if (ENTITY.has(c, COMPONENT.POSITION))
        {
            Position p = (Position)ComponentManager.get(COMPONENT.POSITION).data[c];
            if (getRegion(new Vector3Int(p.x, p.y, p.z), game, out IRegion r))
            {
                r.removeEntity(c);
                return true;
            }
        }
        return false;
    }


    public bool addEntity(uint c, IGame game)
    {
        if (ENTITY.has(c, COMPONENT.POSITION))
        {
           
            Position p = (Position) ComponentManager.get(COMPONENT.POSITION).data[c];
            if (getRegion(new Vector3Int(p.x, p.y, p.z), game, out IRegion r))
                {
                    r.addEntity(c);
                    return true;
                }
        }
        return false;
    }

    public Vector3Int worldPosition(Vector3Int regionPos)
    {
        Vector2Int regionDim = Term.StockDim();
        return new Vector3Int(regionPos.x * (regionDim.x - 1), regionPos.y * (regionDim.y - 1), regionPos.z);
    }

    public Vector3Int regionPosition(Vector3Int position)
    {
        Vector2Int regionDim = Term.StockDim();
        return new Vector3Int(Mathf.FloorToInt(position.x / regionDim.x), Mathf.FloorToInt(position.y / regionDim.y), position.z);
    }

}
