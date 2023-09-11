using System;

[Serializable]
public class Defenses : Component
{
    //public ushort[] saves = new ushort[5];
    public string HD = "1d8";
    public int hpMax = 0;
    public int hp = 0;
    public short AC = 0;
    public float regenRate = .06f;
    public float regenAmt = 0f;
    public STATUS immunities = STATUS.NONE;
}
