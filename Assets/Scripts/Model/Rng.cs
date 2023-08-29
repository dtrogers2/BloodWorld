using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rng
{
    protected int seed;
    protected System.Random rand;
    public Rng(int seed)
    {
        this.seed = seed;
        this.rand = new System.Random(seed);
    }
    public int getSeed() { return this.seed; }
    public void setSeed(int seed) { this.seed = seed; }
    public float srand()
    {
        this.seed = (this.seed * 9301 + 49297) % 233280;
        return (float) this.seed / 233280;
    }

    public int rng(int lower, int higher = 0)
    {
        if (higher == 0)
        {
            higher = lower;
            lower = 0;
        }

        if (lower > higher)
        {
            int swap = lower;
            lower = higher;
            higher = swap;
        }

        //int range = higher - lower;
        //float draw = this.srand() * range;
        int roll = rand.Next(lower, higher);//Mathf.FloorToInt(draw) + lower;
        return roll;
    }

    public int rngC(int lower, int higher)
    {
        return this.rng(lower, higher);
    }

    public Vector2Int rndDir0()
    {
        return new Vector2Int(this.rngC(-1, 1), this.rngC(-1, 1));
    }
    public Vector2Int rndDir(Vector2Int pos)
    {
        return new Vector2Int(pos.x + this.rngC(-1, 1), pos.y + this.rngC(-1, 1));
    }

    public bool oneIn(int n)
    {
        return (this.rng(n) == 0);
    }

    public bool pct(int chance)
    {
        int roll = this.rngC(1, 100);
        return (roll <= chance);
    }
}
