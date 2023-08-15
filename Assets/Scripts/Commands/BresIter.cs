using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BresIter
{
    Stack<Vector2Int> points = new Stack<Vector2Int>();
    int i = 0;
    int shortest = 0;
    int longest = 0;
    int dx1 = 0;
    int dx2 = 0;
    int dy1 = 0;
    int dy2 = 0;
    int numerator = 0;
    int x = 0;
    int y = 0;

    public static BresIter bresIter(Vector3Int p1, Vector3Int p2)
    {
        return new BresIter(p1.x, p1.y, p2.x, p2.y);
    }

    public static BresIter bresIter(int x1, int y1, int x2, int y2)
    {
        return new BresIter(x1, y1, x2, y2);
    }

    public BresIter(int x1, int y1, int x2, int y2)
    {
        this.x = x1;
        this.y = y1;
        int w = x2 - this.x;
        int h = y2 - this.y;
        this.dx1 = 0;
        this.dy1 = 0;
        this.dx2 = 0;
        this.dy2 = 0;
        if (w < 0) { this.dx1 = -1; }
        else if (w > 0) { this.dx1 = 1; }
        if (h < 0) { this.dy1 = -1; }
        else if (h > 0) { this.dy1 = 1; }
        if (w < 0) { this.dx2 = -1; }
        else if (w > 0) { this.dx2 = 1; }
        this.longest = Mathf.Abs(w);
        this.shortest = Mathf.Abs(h);
        if (!(this.longest > this.shortest))
        {
            this.longest = Mathf.Abs(h);
            this.shortest = Mathf.Abs(w);
        if (h < 0) { this.dy2 = -1; }
        else if (h > 0) { this.dy2 = 1; }
        this.dx2 = 0;
        }
        this.numerator = Mathf.FloorToInt(this.longest * 0.5f);
        this.i = 0;
    }
   public bool done() { return !(this.i <= this.longest); }
    public Vector2Int next()
    {
        Vector2Int curPoint = new Vector2Int(this.x, this.y);
        this.points.Push(curPoint);
        this.numerator += this.shortest;
        if (!(this.numerator < this.longest))
        {
            this.numerator -= this.longest;
            this.x += this.dx1;
            this.y += this.dy1;
        }
        else
        {
            this.x += this.dx2;
            this.y += this.dy2;
        }
        ++this.i;
        return curPoint;
    }
    public void iterAll1()
    {
        for (this.i = 0; this.i <= this.longest; this.i++)
        {
            this.next();
        }
    }

    public void iterAll2()
    {
        Vector2Int p;
        for (; !this.done();) { p = this.next(); }
        do { p = this.next(); } while (!this.done());
    }
}
