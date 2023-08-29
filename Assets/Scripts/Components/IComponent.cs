using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct IComponent
{
    public object[] data { get; set; }
    public List<uint> entities { get; set; }

}