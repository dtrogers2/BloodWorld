using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

[Flags]

public enum COMPONENT : uint
{
    //[XmlEnum("COMPONENT.NONE")]
    NONE = 0,
    //[XmlEnum("COMPONENT.POSITION")]
    POSITION = 1,
    //[XmlEnum("COMPONENT.GLYPH")]
    GLYPH = 1 << 1,
    //[XmlEnum("COMPONENT.CREATURE")]
    CREATURE = 1 << 2,
    //[XmlEnum("COMPONENT.CELLSTACK")]
    CELLSTACK = 1 << 3,
    //[XmlEnum("COMPONENT.STAIN")]
    STAIN = 1 << 4,
    //[XmlEnum("COMPONENT.DEFENSES")]
    DEFENSES = 1 << 5,
    //[XmlEnum("COMPONENT.ATTACK")]
    ATTACKS = 1 << 6,
    //[XmlEnum("COMPONENT.ITEM")]
    ITEM = 1 << 7,
}
