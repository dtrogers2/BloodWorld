
using System;
using System.Xml.Serialization;

[Flags]
public enum EQUIPSLOT
{
    [XmlEnum("EQUIPSLOT.NONE")]
    NONE = 0,
    [XmlEnum("EQUIPSLOT.HEAD")]
    HEAD = 1,
    [XmlEnum("EQUIPSLOT.ARMOR")]
    ARMOR = 1 << 1,
    [XmlEnum("EQUIPSLOT.NECKLACE")]
    NECKLACE = 1 << 2,
    [XmlEnum("EQUIPSLOT.BODY")]
    BODY = 1 << 3,
    [XmlEnum("EQUIPSLOT.BACK")]
    BACK = 1 << 4,
    [XmlEnum("EQUIPSLOT.GLOVES")]
    GLOVES = 1 << 5,
    [XmlEnum("EQUIPSLOT.HAND")]
    HAND = 1 << 6,
    [XmlEnum("EQUIPSLOT.TWOHANDED")]
    TWOHANDED = 1 << 7,
    [XmlEnum("EQUIPSLOT.RING")]
    RING = 1 << 8,
    [XmlEnum("EQUIPSLOT.WRIST")]
    WRIST = 1 << 9,
    [XmlEnum("EQUIPSLOT.BOOT")]
    BOOT = 1 << 10,
    [XmlEnum("EQUIPSLOT.NEARBY")]
    NEARBY = 1 << 11,
    [XmlEnum("EQUIPSLOT.QUIVER")]
    QUIVER = 1 << 12,
}
