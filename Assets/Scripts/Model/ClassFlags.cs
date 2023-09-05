using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;

public enum CLASS : uint
{
   NONE = 0, // 0
   MONSTER = 1, // 1
   FIGHTER = 1 << 1, // 2
   MAGICIAN = 1 << 2, // 3
   CLERIC = 1 << 3, // 4
   THIEF = 1 << 4, // 5
   ELF = 1 << 5, // 6
   DWARF = 1 << 6, // 7
   HALFLING = 1 << 7, // 8
}