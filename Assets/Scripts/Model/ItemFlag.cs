
using System;
using System.Xml.Serialization;

[Flags]
public enum ITEMFLAG : uint
{
    [XmlEnum("ITEMFLAG.NONE")]
    NONE  = 0,
    [XmlEnum("ITEMFLAG.CONTAINER")]
    CONTAINER = 1, // 1
    [XmlEnum("ITEMFLAG.FLUID")]
    FLUID = 1 << 1, // 2
    [XmlEnum("ITEMFLAG.ARMOR")]
    ARMOR = 1 << 2, // 4
    [XmlEnum("ITEMFLAG.WEAPON")]
    WEAPON = 1 << 3, // 8
    [XmlEnum("ITEMFLAG.JEWELRY")]
    JEWELRY = 1 << 4, // 16
    [XmlEnum("ITEMFLAG.EVOCABLE")]
    EVOCABLE = 1 << 5, // 32
    [XmlEnum("ITEMFLAG.CHARGES")]
    CHARGES = 1 << 6, // 64
    [XmlEnum("ITEMFLAG.ENCHANTMENT")]
    ENCHANTMENT = 1 << 7, // 128
    [XmlEnum("ITEMFLAG.AMMUNITION")]
    AMMUNITION = 1 << 8, // 256
    [XmlEnum("ITEMFLAG.THROWN")]
    THROWN = 1 << 9, // 512
    [XmlEnum("ITEMFLAG.STACKABLE")]
    STACKABLE = 1 << 10, // 1024
    [XmlEnum("ITEMFLAG.EGO")]
    EGO = 1 << 11, // 2048
    [XmlEnum("ITEMFLAG.CONSUMEABLE")]
    CONSUMABLE = 1 << 12, // 4096
    [XmlEnum("ITEMFLAG.ATTUNE")]
    ATTUNE = 1 << 13, // 8192
    [XmlEnum("ITEMFLAG.CURSED")]
    CURSED = 1 << 14, // 16,384
    [XmlEnum("ITEMFLAG.TREASURE")]
    TREASURE = 1 << 15, // 32,768
    [XmlEnum("ITEMFLAG.IDENTIFIED")]
    IDENTIFIED = 1 << 16, // 65,536
    [XmlEnum("ITEMFLAG.EXPERIENCE")]
    EXPERIENCE = 1 << 17, // 131,072
}