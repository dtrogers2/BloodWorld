using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature
{
    public string name = "None";
    public int moveSpeed = 30;
    public float AP = 0f;
    public int vision = 5;
    public CLASS classes = 0;
    public int[] levels = new int[Enum.GetNames(typeof(CLASS)).Length];
    public int[] exp = new int[Enum.GetNames(typeof(CLASS)).Length];
}
