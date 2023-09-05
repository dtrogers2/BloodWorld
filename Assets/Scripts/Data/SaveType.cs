using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Save
{
    public static SAVE[] SAVETABLE = new SAVE[Enum.GetNames(typeof(CLASS)).Length];
    public static void init()
    {
        // Init NONE
        int index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.NONE);
        SAVETABLE[index] = new SAVE { saves = new[] { new ushort[] { 14, 15, 16, 17, 18 }}}; // NONE
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.MONSTER);
        SAVETABLE[index] = new SAVE 
        { 
            saves = new[] { new ushort[] { 14, 15, 16, 17, 18 },
                            new ushort[] { 12, 13, 14, 15, 16 },
                            new ushort[] { 10, 11, 12, 13, 14 },
                            new ushort[] { 8, 9, 10, 10, 12 },
                            new ushort[] { 6, 7, 8, 8, 10 },
                            new ushort[] { 4, 5, 6, 5, 8 },
                            new ushort[] { 2, 2, 2, 2, 4 },
                            new ushort[] { 2, 2, 2, 2, 2 },

        },}; // MONSTER
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.FIGHTER);
        SAVETABLE[index] = new SAVE
        {
            saves = new[] { new ushort[] { 12, 13, 14, 15, 16 },
                            new ushort[] { 10, 11, 12, 13, 14 },
                            new ushort[] { 8, 9, 10, 10, 12 },
                            new ushort[] { 6, 7, 8, 8, 10 },
                            new ushort[] { 4, 5, 6, 5, 8 },

        },
        }; // FIGHTER
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.MAGICIAN);
        SAVETABLE[index] = new SAVE
        {
            saves = new[] { new ushort[] { 13, 14, 13, 16, 15 },
                            new ushort[] { 11, 12, 11, 14, 12 },
                            new ushort[] { 8, 9, 8, 11, 8 },

        },
        }; // MAGICIAN
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.CLERIC);
        SAVETABLE[index] = new SAVE
        {
            saves = new[] { new ushort[] { 11, 12, 14, 16, 15 },
                            new ushort[] { 9, 10, 12, 14, 12 },
                            new ushort[] { 6, 7, 9, 11, 9 },
                            new ushort[] { 3, 5, 7, 8, 7 },

        },
        }; // CLERIC
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.THIEF);
        SAVETABLE[index] = new SAVE
        {
            saves = new[] { new ushort[] { 13, 14, 13, 16, 15 },
                            new ushort[] { 12, 13, 11, 14, 13 },
                            new ushort[] { 10, 11, 9, 12, 10 },
                            new ushort[] { 8, 9, 7, 10, 8 },

        },
        }; // THEIF
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.DWARF);
        SAVETABLE[index] = new SAVE
        {
            saves = new[] { new ushort[] { 8, 9, 10, 13, 12 },
                            new ushort[] { 6, 7, 8, 10, 10 },
                            new ushort[] { 4, 5, 6, 7, 8 },
                            new ushort[] { 2, 3, 4, 4, 6 },

        },
        }; // DWARF
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.HALFLING);
        SAVETABLE[index] = new SAVE
        {
            saves = new[] { new ushort[] { 8, 9, 10, 13, 12 },
                            new ushort[] { 6, 7, 8, 10, 10 },
                            new ushort[] { 4, 5, 6, 7, 8 },
                            new ushort[] { 2, 3, 4, 4, 6 },

        },
        }; // HALFLING
        index = Array.IndexOf(Enum.GetValues(typeof(CLASS)), CLASS.ELF);
        SAVETABLE[index] = new SAVE
        {
            saves = new[] { new ushort[] { 12, 13, 13, 15, 15 },
                            new ushort[] { 10, 11, 11, 13, 12 },
                            new ushort[] { 8, 9, 9, 10, 10 },
                            new ushort[] { 6, 7, 8, 8, 8 },

        },
        }; // ELF
    }
}

public struct SAVE
{
    public ushort[][] saves; //new int[20][Enum.GetNames(typeof(SAVES)).Length];
}

public enum SAVES
{
    DEATH, // death / poison
    WANDS,
    PARALYSIS, // Paralysis / petrify
    BREATH,
    SPELLS // Spells / rods /staves 
}