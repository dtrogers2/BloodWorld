using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class Ego : Component
{
    public FAC factions = FAC.NONE;
    public short[] reputations = new short[20] { 
        0, // NONE
        0, // LAW
        0, // NEUTRAL
        -200, // CHAOS
        0, // HUMAN
        0, // DWARF
        0, // ELF
        0, // HALFLING
        -400, // GOBLIN
        -400, // ORC
        -800, // UNDEAD
        -475, // INSECT
        -200, // BEAST
        -600, // DRAGON
        0, // ELEMENTAL
        -700, // DEMON
        -475, // GIANT
        -475, // MONSTROSITY
        -475, // OOZE
        0  // PLANT
    };
    public short loyalty = 300;
    public short kinship = 300;
    public MOOD mood = MOOD.NEUTRAL;
}

public enum MOOD
{
    NEUTRAL,
    AGGRESSIVE,
    DOCILE,
    FRIENDLY
}
