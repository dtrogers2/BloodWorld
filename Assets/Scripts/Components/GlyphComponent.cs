using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlyphComponent : ComponentInf
{
    public object[] data { get; } = new object[EntityManager.ENTITIES_DEFAULT];
    public List<uint> entities { get; } = new List<uint>();
}

public struct Glyph
{
    public char c;
    public string color;
}