using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartDataUtils
{
    public static PartData[] HUMANPARTS()
    {
        return new PartData[] {
                new PartData { name = "Head", parttype = PARTTYPE.HEAD, active = true, parentIndex = 0, vital = true },
                new PartData { name = "Torso", parttype = PARTTYPE.BODY, active = true, parentIndex = 0, vital = true },
                new PartData { name = "Left Arm", parttype = PARTTYPE.ARM, active = true, parentIndex = 1, vital = false },
                new PartData { name = "Right Arm", parttype = PARTTYPE.ARM, active = true, parentIndex = 1, vital = false },
                new PartData { name = "Left Leg", parttype = PARTTYPE.LEG, active = true, parentIndex = 1, vital = false },
                new PartData { name = "Right Leg", parttype = PARTTYPE.LEG, active = true, parentIndex = 1, vital = false },
            };
    }

    public static PartData[] FLYERPARTS()
    {
        return new PartData[] {
                new PartData { name = "Head", parttype = PARTTYPE.HEAD, active = true, parentIndex = 0, vital = true },
                new PartData { name = "Body", parttype = PARTTYPE.BODY, active = true, parentIndex = 0, vital = true },
                new PartData { name = "Left Wing", parttype = PARTTYPE.WING, active = true, parentIndex = 1, vital = false },
                new PartData { name = "Right Wing", parttype = PARTTYPE.WING, active = true, parentIndex = 1, vital = false },
            };
    }

    public static PartData[] QUADRAPED()
    {
        return new PartData[] {
                new PartData { name = "Head", parttype = PARTTYPE.HEAD, active = true, parentIndex = 0, vital = true },
                new PartData { name = "Body", parttype = PARTTYPE.BODY, active = true, parentIndex = 0, vital = true },
                new PartData { name = "Left Foreleg", parttype = PARTTYPE.LEG, active = true, parentIndex = 1, vital = false },
                new PartData { name = "Right Foreleg", parttype = PARTTYPE.LEG, active = true, parentIndex = 1, vital = false },
                new PartData { name = "Left Hindleg", parttype = PARTTYPE.LEG, active = true, parentIndex = 1, vital = false },
                new PartData { name = "Right Hindleg", parttype = PARTTYPE.LEG, active = true, parentIndex = 1, vital = false },
            };
    }
}
public struct PartData
{
    public string name;
    public PARTTYPE parttype;
    public short parentIndex;
    public bool active;
    public bool vital;
}
