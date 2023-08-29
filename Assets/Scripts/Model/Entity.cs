using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEntity
{
    public Vector3Int position { get; set; }
    public string name { get; }
    public char glyph { get; }
    public string color { get; }
}
