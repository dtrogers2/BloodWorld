using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Element
{
    None = 0,
    Blood = 1,
    Stone = 2,
    Water = 4,
    Glass = 8,
}

public enum State
{
    None = 0,
    Solid = 1,
    Liquid = 2,
    Gas = 3,
}

public readonly struct ElementColor
{
    public const string None = ColorHex.Black;
    public const string Blood = ColorHex.Red_Bright;
    public const string Stone = ColorHex.White_Dark;
    public const string Water = ColorHex.Blue_Bright;
    public const string Glass = ColorHex.White;

    public static string ColorFromElement(Element element)
    {
        switch (element)
        {
            case Element.None: return ElementColor.None;
            case Element.Blood: return ElementColor.Blood;
            case Element.Stone: return ElementColor.Stone;
            case Element.Water: return ElementColor.Water;
            case Element.Glass: return ElementColor.Glass;
            default: return ElementColor.None;
        }
    }
}