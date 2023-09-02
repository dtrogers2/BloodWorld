using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface MapDrawerIF
{
    public uint setp(Vector2Int pos, uint env);
    public uint get(Vector2Int pos);
    public void carve(Vector2Int pos, ENV env, ENV hard);
    public void carve(Vector2Int pos, ENV env, params ENV[] hard);
    public Vector2Int dim { get; set; }
    IRegion map { get; set; }
    public void render();
    public void fillMap(uint initEnv);
    public bool legal(Vector2Int pos);
}
