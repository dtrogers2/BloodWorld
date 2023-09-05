using System;
using System.Collections;
using System.Collections.Generic;

[Flags]
public enum COMPONENT : uint
{
    NONE = 0,
    POSITION = 1,
    GLYPH = 1 << 1,
    CREATURE = 1 << 2,
    CELLSTACK = 1 << 3,
    STAIN = 1 << 4,
    DEFENSES = 1 << 5,
    ATTACK = 1 << 6,
}
