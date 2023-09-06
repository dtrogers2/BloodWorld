using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEMFLAG : uint
{
    NONE  = 0,
    CONTAINER = 1,
    FLUID = 1 << 1,
    ARMOR = 1 << 2,
    WEAPON = 1 << 3,
    JEWELRY = 1 << 4,
    EVOCABLE = 1 << 5,
    CHARGES = 1 << 6,
    ENCHANTMENT = 1 << 7,
    AMMUNITION = 1 << 8,
    THROWN = 1 << 9,
    STACKABLE = 1 << 10,
    EGO = 1 << 11,
    CONSUMABLE = 1 << 12,
    ATTUNE = 1 << 13,
    CURSED = 1 << 14,
    TREASURE = 1 << 15,
    IDENTIFIED = 1 << 16,
    EXPERIENCE = 1 << 17,
}