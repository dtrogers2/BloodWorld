using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attacks
{
    public float baseAtkRate;
    public Attack[] attacks;
    public uint atkUsed;
}

public struct Attack
{
    public string name;
    public string dmgDice;
    public EFFECT[] effects;
    public float atkRate;
}
