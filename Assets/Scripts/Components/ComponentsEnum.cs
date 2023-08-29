using System;
using System.Collections;
using System.Collections.Generic;

[Flags]
public enum COMPONENT
{
    NONE = 0,
    POSITION = 1 << 1,
    GLYPH = 1 << 2,
    CREATURE = 1 << 3,
}
