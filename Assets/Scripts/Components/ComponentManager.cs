using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public static class ComponentManager
{
     public static IComponent[] COMPONENTS = new IComponent[Enum.GetNames(typeof(COMPONENT)).Length];
    
    public static void init()
    {
        for (int i = 0; i < COMPONENTS.Length; i++)
        {
            COMPONENTS[i].data = new object[EntityManager.ENTITIES_DEFAULT];
            COMPONENTS[i].entities = new List<uint>();
        }
    }

    public static IComponent get(COMPONENT c)
    {
        int index = Array.IndexOf(Enum.GetValues(c.GetType()), c);
        return COMPONENTS[index];
    }
}
