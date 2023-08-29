using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionComponent : ComponentInf
{
    public object[] data { get; } = new object[EntityManager.ENTITIES_DEFAULT];
    public List<uint> entities { get; } = new List<uint>();

}

public struct Position
{
    public int x;
    public int y;
    public int z;
}