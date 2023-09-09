using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

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
    ATTACKS = 1 << 6,
    ITEM = 1 << 7,
    EGO = 1 << 8,
}
