using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[XmlInclude(typeof(Defenses))]
public class Defenses
{
    //public ushort[] saves = new ushort[5];
    //[XmlElement(typeof(string))]
    public string HD = "1d4";
    //[XmlElement(typeof(int))]
    public int hpMax = 0;
    //[XmlElement(typeof(int))]
    public int hp = 0;
    //[XmlElement(typeof(int))]
    public short AC = 9;
    public STATUS immunities = STATUS.NONE;
}
