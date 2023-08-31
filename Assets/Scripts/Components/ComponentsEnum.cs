using System;
using System.Collections;
using System.Collections.Generic;

[Flags]
public enum COMPONENT
{
    NONE = 0,
    POSITION = 1,
    GLYPH = 1 << 1,
    CREATURE = 1 << 2,
    CELLSTACK = 1 << 3,
    PATH = 1 << 4,
    STAIN = 1 << 5,
}
