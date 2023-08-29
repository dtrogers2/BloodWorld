using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
///  If new flags are added, remember to update Region.cs getCellEntity and setCellEntity methods to shift to the correct amount of bits!! 
/// </summary>
[Flags]
public enum CELLFLAG : uint
{
    EMPTY = 0,

    SEEN = 1,  // 1
    BLOCKED = 1 << 1, // 2
    OPAQUE = 1 << 2, // 4
    EXPLORE_HORIZON = 1 << 3,//8
    MORE_ITEMS = 1 << 4, //16
}
