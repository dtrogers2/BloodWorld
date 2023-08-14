using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Tile : IComparer
{
    public Wall wall;
    public TermChar floor;
    public Creature creature;
    public bool lit;
    public Vector2Int position;

    public int gScore { set; get; }
    public int hScore { set; get; }
    public bool visited { set; get; }
    public Tile nodeParent { set; get; }

    public Tile(TermChar floor, int x, int y)
    {
        this.floor = floor;
        this.position = new Vector2Int(x, y);
        resetNode();
    }

    public bool blocks()
    {
        
        if (wall != null) return wall.blocks;
        return creature != null;
    }
    public bool opaque()
    {
        return wall != null;
    }

    public TermChar glyph()
    {
        char c = (this.wall != null) ? wall.glyph : ( this.creature != null) ? creature.glyph :  this.floor.c;
        string fg = (this.wall != null) ? wall.color : (this.creature != null) ? creature.color :  this.floor.foreground;
        string bg = (this.creature != null) ? creature.bg() : this.floor.background;
        return new TermChar { c = c, background = bg , foreground = fg, special = ""};
    }

    public void setG(Tile previous)
    {
        gScore = previous.getG() + 1;
    }

    public int getG()
    {
        return gScore;
    }

    public void setH(Tile goal)
    {
        hScore = distanceBetween(goal);
    }

    public int getH()
    {
        return hScore;
    }

    public int distanceBetween(Tile goal)
    {
        return Mathf.Abs(goal.position.x - position.x) + Mathf.Abs(goal.position.y - position.y);
    }
    int IComparer.Compare(object a, object b)
    {
        Tile center = (Tile) b;
        Tile n2 = (Tile) a;
        double dist1 = distanceBetween(center);
        double dist2 = n2.distanceBetween(center);
        if (dist1 > dist2) return 1;
        if (dist1 < dist2) return -1;
        else return 0;
    }

    public void resetNode()
    {
        gScore = int.MaxValue;
        hScore = int.MaxValue;
        nodeParent = this;
        visited = false;
    }
}
