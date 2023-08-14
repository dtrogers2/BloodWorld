using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct ColorHex
{
    public const string Black = "#0c0c0c";
    public const string Red_Dark = "#c50f1f";
    public const string Green_Dark = "#13a10e";
    public const string Yellow_Dark = "#c19c00";
    public const string Blue_Dark = "#0037da";
    public const string Magenta_Dark = "#881798";
    public const string Cyan_Dark = "#3a96dd";
    public const string White_Dark = "#cccccc";
    public const string Black_Bright = "#767676";
    public const string Red_Bright = "#e74856";
    public const string Green_Bright = "#16c60c";
    public const string Yellow_Bright = "#f9f1a5";
    public const string Blue_Bright = "#3b78ff";
    public const string Magenta_Bright = "#b4009e";
    public const string Cyan_Bright = "#61d6d6";
    public const string White = "#f2f2f2";
}

public readonly struct ForegroundCode
{
    public const string Black = "\u001b[30m";
    public const string Red_Dark = "\u001b[31m";
    public const string Green_Dark = "\u001b[32m";
    public const string Yellow_Dark = "\u001b[33m";
    public const string Blue_Dark = "\u001b[34m";
    public const string Magenta_Dark = "\u001b[35m";
    public const string Cyan_Dark = "\u001b[36m";
    public const string White_Dark = "\u001b[37m";
    public const string Black_Bright = "\u001b[90m";
    public const string Red_Bright = "\u001b[91m";
    public const string Green_Bright = "\u001b[92m";
    public const string Yellow_Bright = "\u001b[93m";
    public const string Blue_Bright = "\u001b[94m";
    public const string Magenta_Bright = "\u001b[95m";
    public const string Cyan_Bright = "\u001b[96m";
    public const string White = "\u001b[97m";
}

public readonly struct BackgroundCode
{
    public const string Black = "\u001b[40m";
    public const string Red_Dark = "\u001b[41m";
    public const string Green_Dark = "\u001b[42m";
    public const string Yellow_Dark = "\u001b[43m";
    public const string Blue_Dark = "\u001b[44m";
    public const string Magenta_Dark = "\u001b[45m";
    public const string Cyan_Dark = "\u001b[46m";
    public const string White_Dark = "\u001b[47m";
    public const string Black_Bright = "\u001b[100m";
    public const string Red_Bright = "\u001b[101m";
    public const string Green_Bright = "\u001b[102m";
    public const string Yellow_Bright = "\u001b[103m";
    public const string Blue_Bright = "\u001b[104m";
    public const string Magenta_Bright = "\u001b[105m";
    public const string Cyan_Bright = "\u001b[106m";
    public const string White = "\u001b[107m";
}

public readonly struct SpecialCode
{
    public const string Default = "\u001b[0m";
    public const string Bold = "\u001b[1m";
    public const string Underline = "\u001b[4m";
    public const string NoUnderline = "\u001b[24m";
    public const string Reverse = "\u001b[7m";
    public const string NoReverse = "\u001b[27m";
}