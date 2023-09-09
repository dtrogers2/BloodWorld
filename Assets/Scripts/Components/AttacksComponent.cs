public class Attacks : Component
{
    //[XmlElement(typeof(float))]
    public float baseAtkRate = 1f;
    //[XmlElement(typeof(Attack[]))]
    public Attack[] attacks = new Attack[] { new Attack { atkRate = 1f, dmgDice = "1d6", name = "strike" } };
    //[XmlElement(typeof(uint))]
    public uint atkUsed = 0;
}
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
