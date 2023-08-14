using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : IEntity
{
    public Vector3Int position {get;set;}

    public string name { get; set; }

    public char glyph { get; set; } = '#';

    public string color { get; set; } = ColorHex.White_Dark;
    public bool opaque = true;
    public bool blocks = true;

    public Wall(Vector3Int position, string name, bool opaque, bool blocks, TermChar termChar)
    {
        this.position = position;
        this.name = name;
        this.glyph = termChar.c;
        this.color = termChar.foreground;
        this.opaque = opaque;
        this.blocks = blocks;
    }

    public Wall(int x, int y, int z, string name, bool opaque, bool blocks, TermChar termChar)
    {
        this.position = new Vector3Int(x, y, z);

        this.name = name;
        this.glyph = termChar.c;
        this.color = termChar.foreground;
        this.opaque = opaque;
        this.blocks = blocks;
    }
}
