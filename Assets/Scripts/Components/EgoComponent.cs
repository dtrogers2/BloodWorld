using System;
[Serializable]
public class Ego : Component
{
    public FAC factions = FAC.NONE;
    public short[] reputations = new short[20] { 
        -200, // NONE
        0, // LAW
        0, // NEUTRAL
        -200, // CHAOS
        -200, // HUMAN
        -200, // DWARF
        -200, // ELF
        -200, // HALFLING
        -400, // GOBLIN
        -400, // ORC
        -800, // UNDEAD
        -475, // INSECT
        -200, // BEAST
        -600, // DRAGON
        -200, // ELEMENTAL
        -700, // DEMON
        -475, // GIANT
        -475, // MONSTROSITY
        -475, // OOZE
        -200  // PLANT
    };
    public short loyalty = 300;
    public short kinship = 300;
    public MOOD mood = MOOD.NEUTRAL;
}

public static class EgoUtils
{
    public static int MoodAdj(uint id)
    {
        int moodAdj = 0;
        if (ENTITY.has(id, COMPONENT.EGO))
        {
            Ego e = (Ego)ComponentManager.get(COMPONENT.EGO).data[id];
            moodAdj = (e.mood == MOOD.FRIENDLY) ? 250 : (e.mood == MOOD.DOCILE) ? 100 : (e.mood == MOOD.NEUTRAL) ? 0 : -250;
        }
        return moodAdj;
    }
}

public enum MOOD
{
    NEUTRAL,
    AGGRESSIVE,
    DOCILE,
    FRIENDLY
}
