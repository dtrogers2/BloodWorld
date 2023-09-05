using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum EGO : uint
{
   NONE = 0,
   LOYALTY = 1,
   FAME = 1 << 1,
   LAW = 1 << 2,
   NEUTRAL = 1 << 3,
   CHAOS = 1 << 4,
   HUMANOID = 1 << 5,
   HUMAN = 1 << 6,
   DWARF = 1 << 7,
   ELF = 1 << 8,
   HALFLING = 1 << 9,
   GOBLIN = 1 << 10,
   ORC = 1 << 11,
   UNDEAD = 1 << 12,
   INSECT = 1 << 13,
   BEAST = 1 << 14,
   DRAGON = 1 << 15,
   ELEMENTAL = 1 << 16,
   DEMON = 1 << 17,
   GIANT = 1 << 18,
   MONSTROSITY = 1 << 19,
   OOZE = 1 << 20,
}
