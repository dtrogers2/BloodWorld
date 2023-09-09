using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class EgoComponent : Component
{
    EGO factions;
    short[] reputations = new short[Enum.GetNames(typeof(EGO)).Length];
}
