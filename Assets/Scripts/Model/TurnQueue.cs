using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnQueue
{
    public List<uint> creatures = new List<uint>();
    public uint cur() { return creatures[0]; }
    public void push(uint c) { creatures.Add(c); }
    public uint pop() { uint c = creatures[0]; creatures.RemoveAt(0); return c; }
    public bool remove(uint c)
    {
        int i = creatures.IndexOf(c);
        if (i == -1) return false;  
        creatures.RemoveAt(i);
        return true;
    }
    public void front(uint c)
    {
        creatures.Remove(c);
        creatures.Insert(0, c);
    }

    public uint next()
    {
        push(pop());
        return cur();
    }
}
