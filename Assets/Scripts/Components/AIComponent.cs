using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class AI
{
    public STATE state = STATE.IDLE;
    public uint leader = 0;
    public uint target = 0;
    public Position pos = new Position { x = 0, y = 0, z = 0};
    public int memoryMax = 9;
    public int memory = 9;

}


public enum STATE
{
    IDLE,
    REST,
    WANDER,
    INVESTIGATE,
    FOLLOW,
    CHASE
}

