using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Flags]
public enum FAC : uint
{
    [XmlEnum("FAC.NONE")]
    NONE = 0,
    [XmlEnum("FAC.LAW")]
    LAW = 1,
    [XmlEnum("FAC.NEUTRAL")]
    NEUTRAL = 1 << 1,
    [XmlEnum("FAC.CHAOTIC")]
    CHAOS = 1 << 2,
    [XmlEnum("FAC.HUMAN")]
    HUMAN = 1 << 3,
    [XmlEnum("FAC.DWARF")]
    DWARF = 1 << 4,
    [XmlEnum("FAC.ELF")]
    ELF = 1 << 5,
    [XmlEnum("FAC.HALFLING")]
    HALFLING = 1 << 6,
    [XmlEnum("FAC.GOBLIN")]
    GOBLIN = 1 << 7,
    [XmlEnum("FAC.ORC")]
    ORC = 1 << 8,
    [XmlEnum("FAC.UNDEAD")]
    UNDEAD = 1 << 9,
    [XmlEnum("FAC.INSECT")]
    INSECT = 1 << 10,
    [XmlEnum("FAC.BEAST")]
    BEAST = 1 << 11,
    [XmlEnum("FAC.DRAGON")]
    DRAGON = 1 << 12,
    [XmlEnum("FAC.ELEMENTAL")]
    ELEMENTAL = 1 << 13,
    [XmlEnum("FAC.DEMON")]
    DEMON = 1 << 14,
    [XmlEnum("FAC.GIANT")]
    GIANT = 1 << 15,
    [XmlEnum("FAC.MONSTROSITY")]
    MONSTROSITY = 1 << 16,
    [XmlEnum("FAC.OOZE")]
    OOZE = 1 << 17,
    [XmlEnum("FAC.PLANT")]
    PLANT = 1 << 18,
}
