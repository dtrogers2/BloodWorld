using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlInclude(typeof(Creature))]
public class Creature
{
    //[XmlText(typeof(string))]
    public string name = "None";
    //[XmlElement(typeof(int))]
    public int moveSpeed = 30;
    //[XmlElement(typeof(float))]
    public float AP = 0f;
    //[XmlElement(typeof(int))]
    public int vision = 8;
    //[XmlElement(typeof(int[]))]
    public int[] exp = new int[Enum.GetNames(typeof(SKILLS)).Length];
}
