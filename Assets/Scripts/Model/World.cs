using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World
{
    Vector3Int worldDim;
    IRegion[,,] regions;

    public World(Vector3Int worldDim)
    {
        this.worldDim = worldDim;
        regions = new IRegion[worldDim.x, worldDim.y, worldDim.z];
    }

    public IRegion curMap(IGame game)
    {
        return getRegion(game.player.position, game);
    }

    public IRegion getRegion(Vector3Int pos, IGame game)
    {
        Vector3Int rP = regionPosition(pos);
        if (!hasRegion(rP))
        {
            IRegion map = game.build.makeLevel(game.rng, rP);
            add(map, rP);
        }
        return this.regions[rP.x, rP.y, rP.z];
    }

    public bool getTile(Vector3Int worldPos, IGame game, out Tile tile)
    {
        tile = null;
        TermChar outside = DrawScreen.outside;
        Vector2Int stockTerm = Term.StockDim();
        if (worldPos.x >= 0 && worldPos.x < worldDim.x * stockTerm.x && worldPos.y >= 0 && worldPos.y < worldDim.y * stockTerm.y && worldPos.z >= 0 && worldPos.z < worldDim.z) 
        {
            IRegion region = getRegion(worldPos, game);
            Vector3Int localPos = localizePosition(worldPos);
            tile = region.tile(localPos.x, localPos.y);
            return true;
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
        if (!hasRegion(regionPosition(dest))) return false;
        IRegion destRegion = getRegion(dest, game);
        if (destRegion == null) return false;

        IRegion curRegion = getRegion(c.position, game);
        Vector3Int localSrc = localizePosition(c.position);
        Vector3Int localDest = localizePosition(dest);

        if (!destRegion.blocked(localDest.x, localDest.y))
        {
            if (curRegion != destRegion)
            {
                creatureSwitchRegion(c, dest, game);
            } else
            {
                curRegion.moveEntity(c, dest, localSrc, localDest);
            }
            return true;
        }
        return false;
    }
    public void creatureSwitchRegion(Creature c, Vector3Int newPos, IGame game)
    {
        Vector3Int localSrc = localizePosition(c.position);
        Vector3Int localDest = localizePosition(newPos);
        curMap(game).removeEntity(c, localSrc.x, localSrc.y);
        c.position = newPos;
        getRegion(newPos, game).enterMap(c, localDest.x, localDest.y);
    }

    public Vector3Int regionPosition(Vector3Int position)
    {
        Vector2Int regionDim = Term.StockDim();
        return new Vector3Int(Mathf.FloorToInt(position.x / regionDim.x), Mathf.FloorToInt(position.y / regionDim.y), position.z);
    }


    public Vector3Int localizePosition(Vector3Int worldPosition)
    {
        Vector2Int regionDim = Term.StockDim();
        return new Vector3Int(worldPosition.x % regionDim.x, worldPosition.y % regionDim.y, worldPosition.z);
    }

}
