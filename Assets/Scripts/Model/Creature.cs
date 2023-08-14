using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Creature : IEntity
{
    public Vector3Int position { get; set; }

    public string name { get; private set; }

    public char glyph { get; private set; }

    public string color { get; private set; }
    public Status status { get; private set; }

    public float baseAtkCost = 1.0f;
    public float baseMoveCost = 1.0f;
    public float actionCost = 0f;
    public int hp, maxhp;
    public Creature(Vector3Int position, string name, TermChar termChar)
    {
        this.position = position;
        this.name = name;
        this.glyph = termChar.c;
        this.color = termChar.foreground;
        maxhp = 5;
        hp = maxhp;
    }

    public string bg()
    {
        switch (this.status)
        {
            default: return ColorHex.Black;
        }
    }

    public bool alive()
    {
        return hp > 0;
    }

    public bool canAct()
    {
        return actionCost <= 0f;
    }
}
