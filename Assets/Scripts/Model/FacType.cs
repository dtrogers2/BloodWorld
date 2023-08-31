using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Flags]
public enum FacType 
{
   NONE = 0,
   BLOOD = 1,
   OIL = 1 << 1,
   PREDATORS = 1 << 2,
   GRAZERS = 1 << 3,
   DEAD = 1 << 3,
   GODS = 1 << 4,
   GORGONS = 1 << 5,
   HUMANS = 1 << 6,
   LEECHS = 1 << 7,
   MACHINES = 1 << 8,
   MOUND = 1 << 9,
   MUTANTS = 1 << 10,
   OUTSIDERS = 1 << 11,
   WRETCHES = 1 << 12,
   CYBORG = 1 << 13,
}
