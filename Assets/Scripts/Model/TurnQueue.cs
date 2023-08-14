using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnQueue
{
    public List<Creature> creatures = new List<Creature>();
    public Creature cur() { return creatures[0]; }
    public void push(Creature c) { creatures.Add(c); }
    public Creature pop() { Creature c = creatures[0]; creatures.RemoveAt(0); return c; }
    public bool remove(Creature c)
    {
        int i = creatures.IndexOf(c);
        if (i == -1) return false;  
        creatures.RemoveAt(i);
        return true;
    }
    public void front(Creature c)
    {
        creatures.Remove(c);
        creatures.Insert(0, c);
    }

    public Creature next()
    {
        push(pop());
        return cur();
    }
}
