using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Flags]
public enum EGO : uint
{
    [XmlEnum("EGO.NONE")]
    NONE = 0,
    [XmlEnum("EGO.LAW")]
    LAW = 1,
    [XmlEnum("EGO.NEUTRAL")]
    NEUTRAL = 1 << 1,
    [XmlEnum("EGO.CHAOTIC")]
    CHAOS = 1 << 2,
    [XmlEnum("EGO.HUMAN")]
    HUMAN = 1 << 4,
    [XmlEnum("EGO.DWARF")]
    DWARF = 1 << 5,
    [XmlEnum("EGO.ELF")]
    ELF = 1 << 6,
    [XmlEnum("EGO.HALFLING")]
    HALFLING = 1 << 7,
    [XmlEnum("EGO.GOBLIN")]
    GOBLIN = 1 << 8,
    [XmlEnum("EGO.ORC")]
    ORC = 1 << 9,
    [XmlEnum("EGO.UNDEAD")]
    UNDEAD = 1 << 10,
    [XmlEnum("EGO.INSECT")]
    INSECT = 1 << 11,
    [XmlEnum("EGO.BEAST")]
    BEAST = 1 << 12,
    [XmlEnum("EGO.DRAGON")]
    DRAGON = 1 << 13,
    [XmlEnum("EGO.ELEMENTAL")]
    ELEMENTAL = 1 << 14,
    [XmlEnum("EGO.DEMON")]
    DEMON = 1 << 15,
    [XmlEnum("EGO.GIANT")]
    GIANT = 1 << 16,
    [XmlEnum("EGO.MONSTROSITY")]
    MONSTROSITY = 1 << 17,
    [XmlEnum("EGO.OOZE")]
    OOZE = 1 << 18,
    [XmlEnum("EGO.PLANT")]
    PLANT = 1 << 19,
}
