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

    
}
