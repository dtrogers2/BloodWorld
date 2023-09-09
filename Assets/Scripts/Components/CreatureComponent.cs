using System;
public class Creature : Component
{
    public string name = "None";
    public int moveSpeed = 30;
    public float AP = 0f;
    public int vision = 8;
    public int[] exp = new int[Enum.GetNames(typeof(SKILLS)).Length];
}
