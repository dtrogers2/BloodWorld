using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlInclude(typeof(Attacks))]
public class Attacks
{
    //[XmlElement(typeof(float))]
    public float baseAtkRate;
    //[XmlElement(typeof(Attack[]))]
    public Attack[] attacks;
    //[XmlElement(typeof(uint))]
    public uint atkUsed;
}

[XmlInclude(typeof(Attack))]
public struct Attack
{
    //[XmlElement(typeof(string))]
    public string name;
    //[XmlElement(typeof(string))]
    public string dmgDice;
    //public EFFECT[] effects;
    //[XmlElement(typeof(float))]
    public float atkRate;
}
