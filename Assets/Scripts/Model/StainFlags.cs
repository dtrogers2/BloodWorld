using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum STAIN
{
    NONE = 0,
    WATER = 1,
    BLOOD = 1 << 2,
    OIL = 1 << 3,
    SOOT = 1 << 4,
    SLIME = 1 << 5,
    RUST = 1 << 6,
    DUST = 1 << 7,

    
}
