using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ComponentInf
{
    public object[] data { get; }
    public List<uint> entities { get; }

}