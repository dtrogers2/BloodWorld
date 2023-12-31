using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public enum COLOR: uint
{
    [XmlEnum("COLOR.BLACK")]
    Black = 0x0C0C0C,
    [XmlEnum("COLOR.REDDARK")]
    RedDark = 0xC50F1F,
    [XmlEnum("COLOR.GREENDARK")]
    GreenDark = 0x13A10E,
    [XmlEnum("COLOR.YELLOWDARK")]
    YellowDark = 0xC19C00,
    [XmlEnum("COLOR.BLUEDARK")]
    BlueDark = 0x0037DA,
    [XmlEnum("COLOR.MAGENTADARK")]
    MagentaDark = 0x881798,
    [XmlEnum("COLOR.CYANDARK")]
    CyanDark = 0x3A96DD,
    [XmlEnum("COLOR.GRAY")]
    Gray = 0xCCCCCC,
    [XmlEnum("COLOR.GRAYDARK")]
    GrayDark = 0x767676,
    [XmlEnum("COLOR.RED")]
    Red = 0xE74856,
    [XmlEnum("COLOR.GREEN")]
    Green = 0x16C60C,
    [XmlEnum("COLOR.YELLOW")]
    Yellow = 0xF9F1A5,
    [XmlEnum("COLOR.BLUE")]
    Blue = 0x3B78FF,
    [XmlEnum("COLOR.MAGENTA")]
    Magenta = 0xB4009E,
    [XmlEnum("COLOR.CYAN")]
    Cyan = 0x61D6D6,
    [XmlEnum("COLOR.WHITE")]
    White = 0xF2F2F2,
}
public readonly struct ColorHex
{
    [XmlEnum("ColorHex.Black")]
    public const string Black = "#0c0c0c";
    [XmlEnum("ColorHex.RedDark")]
    public const string RedDark = "#c50f1f";
    [XmlEnum("ColorHex.GreenDark")]
    public const string GreenDark = "#13a10e";
    [XmlEnum("ColorHex.YellowDark")]
    public const string YellowDark = "#c19c00";
    [XmlEnum("ColorHex.BlueDark")]
    public const string BlueDark = "#0037da";
    [XmlEnum("ColorHex.MagentaDark")]
    public const string MagentaDark = "#881798";
    [XmlEnum("ColorHex.CyanDark")]
    public const string CyanDark = "#3a96dd";
    [XmlEnum("ColorHex.Gray")]
    public const string Gray = "#cccccc";
    [XmlEnum("ColorHex.GrayDark")]
    public const string GrayDark = "#767676";
    [XmlEnum("ColorHex.Red")]
    public const string Red = "#e74856";
    [XmlEnum("ColorHex.Green")]
    public const string Green = "#16c60c";
    [XmlEnum("ColorHex.Yellow")]
    public const string Yellow = "#f9f1a5";
    [XmlEnum("ColorHex.Blue")]
    public const string Blue = "#3b78ff";
    [XmlEnum("ColorHex.Magenta")]
    public const string Magenta = "#b4009e";
    [XmlEnum("ColorHex.Cyan")]
    public const string Cyan = "#61d6d6";
    [XmlEnum("ColorHex.White")]
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