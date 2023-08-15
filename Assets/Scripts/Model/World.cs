using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;
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

    public IRegion playerRegion(IGame game)
    {
        IRegion r = null;
        getRegion(game.player.position, game, out r);
        return r;
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

    public bool getTile(Vector3Int worldPos, IGame game, out Tile tile)
    {
        tile = null;
        Vector2Int stockTerm = Term.StockDim();
        if (worldPos.x >= 0 && worldPos.x < worldDim.x * stockTerm.x && worldPos.y >= 0 && worldPos.y < worldDim.y * stockTerm.y && worldPos.z >= 0 && worldPos.z < worldDim.z)
        {
            if (getRegion(worldPos, game, out IRegion r))
            {
                tile = r.tile(worldPos);
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
    public bool moveEntity(Creature c, Vector3Int dest, IGame game)
    {
        if (getTile(dest, game, out Tile tile))
        {
            if (!tile.blocks())
            {
                if (removeEntity(c, game))
                {
                    c.position = dest;
                    if (addEntity(c, game))
                    {
                        return true;
                    } else
                    {
                        throw new System.Exception("Entity was removed but not added in moveEntity()!!!");
                    }
                }
                return false;
            }
        }
        return false;
    }

    public bool removeEntity(Creature c, IGame game)
    {
        if (getRegion(c.position, game, out IRegion r))
        {
            r.removeEntity(c);
            return true;
        }
        return false;
    }

    public bool addEntity(Creature c, IGame game)
    {
        if (getRegion(c.position, game, out IRegion r))
        {
            r.addEntity(c);
            return true;
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

    public Tile getUnblockedGoal(IGame game, Tile center, Tile start)
    {
        int loopMax = 3;
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
                    if (getTile(new Vector3Int(x, y), game, out Tile t))
                    {
                        if (!t.blocks())
                        {

                            if (!found)
                            {
                                newGoal = t;
                                found = true;
                            }
                            if (found == true)
                            {
                                if ((t.distanceBetween(center) <= newGoal.distanceBetween(center)) && t.distanceBetween(start) <= newGoal.distanceBetween(start))
                                {
                                    newGoal = t;
                                }
                            }
                        }
                    } else
                    {
                        continue;
                    }
                    
                }
            }
            if (xmin - 1 > 0) xmin--;
            if (xmax + 1 < game.world.worldDim.x * Term.StockDim().x) xmax++;
            if (ymin - 1 > 0) ymin--;
            if (ymax + 1 < game.world.worldDim.x * Term.StockDim().y) ymax++;
            loopMax--;
            if (loopMax == 0) return null;
        } while (found == false);
        return newGoal;
    }


    public List<Tile> getNeighbors(IGame game, Vector3Int position)
    {
        List<Tile> retList = new List<Tile>();
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (y == 0 && x == 0) continue;
                if (getTile(position + new Vector3Int(x, y, position.z), game, out Tile t))
                {
                    if (!t.blocks()) retList.Add(t);
                }
            }
        }
        return retList;
    }
}
