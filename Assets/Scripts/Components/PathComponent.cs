using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path
{
    public Stack<Vector3Int> path { get; set; }
    public int gScore {get; set;}
    public int pathTurns {get; set;}
}
