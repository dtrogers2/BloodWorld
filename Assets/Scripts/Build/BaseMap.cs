using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseMap : MapDrawerIF
{

    public BaseMap(Vector2Int dim, IRegion map)
    {
        this.dim = dim;
        this.map = map;
    }

    public Vector2Int dim { get; set ; }
    public IRegion map { get; set ; }

    public void carve(Vector2Int pos, ENV env, ENV hard)
    {
        if (map.getCellEntity((Vector3Int)pos) == (uint)hard) return;
        map.setCellEntity((uint)env, (Vector3Int)pos);
        if (env == ENV.WALL) map.setCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, (Vector3Int)pos);
        else map.delCellFlags(CELLFLAG.BLOCKED | CELLFLAG.OPAQUE, (Vector3Int)pos);
    }

    public void fillMap(uint initEnv)
    {
        for (int y = 0; y < dim.y; y++)
        {
            for (int x = 0; x < dim.x; x++)
            {
                setp(new Vector2Int(x, y), initEnv);
            }
        }
    }

    public uint get(Vector2Int pos)
    {
        return map.getCellEntity((Vector3Int)pos);
    }

    public void set(Vector2Int pos, uint entity)
    {
        setp(pos, entity);
    }

    public bool legal(Vector2Int pos)
    {
        return pos.x >= 0 && pos.y >= 0 && pos.x < dim.x && pos.y < dim.y;
    }

    public void render()
    {
        throw new System.NotImplementedException();
    }

    public uint setp(Vector2Int pos, uint env)
    {
        if (!legal(pos)) { throw new System.Exception($"{pos} not legal"); }
        map.setCellEntity(env, (Vector3Int) pos);
        return env;
    }
}
