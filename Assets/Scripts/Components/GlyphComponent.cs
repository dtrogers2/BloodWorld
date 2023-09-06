using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlInclude(typeof(Glyph))]
[Serializable]
public class Glyph
{
    //[XmlElement(typeof(char))]
    public char c;
    //[XmlElement(typeof(string))]
    public string color;
}