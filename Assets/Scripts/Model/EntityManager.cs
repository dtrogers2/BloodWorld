using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public static class EntityManager
{
    public static uint ENTITIES_DEFAULT = 100000;
    public static uint curLength = 0;
    public static uint[] entities = new uint[ENTITIES_DEFAULT];
    public static Stack<uint> reuse = new Stack<uint>();

    public static void set(uint eId, uint bitset)
    {
        entities[eId] = bitset;
    }

    public static uint get(uint eId)
    {
        return entities[eId];
    }

    public static void clear(uint eId)
    {
        entities[eId] = (uint) COMPONENT.NONE;
        reuse.Push(eId);
    }
    public static uint create()
    {
        if (reuse.Count > 0)
        {
            return reuse.Pop();
        }
        return curLength++;
    }

    public static void reset()
    {
        entities = new uint[ENTITIES_DEFAULT];
        curLength = 0;
    }
}


public struct ENTITY
{
    public static void subscribe(uint id, object data, COMPONENT component)
    {
        uint eBits = EntityManager.entities[id];
        uint newSet = bitSet(eBits, (uint) component);
        EntityManager.entities[id] = newSet;
        int index = Array.IndexOf(Enum.GetValues(component.GetType()), component);
        ComponentManager.COMPONENTS[index].data[id] = data;
        ComponentManager.COMPONENTS[index].entities.Add(id);
    }
    public static void subscribe(uint id, object[] data, params COMPONENT[] components)
    {
        if (data.Length != components.Length) { throw new Exception($"subscribe() data length differs from components length!!"); }
        for (int i = 0; i < components.Length; i++)
        {
            subscribe(id, data[i], components[i]);
        }
    }

    public static void unsubscribe(uint id, COMPONENT component)
    {
        EntityManager.entities[id] = bitDel(EntityManager.entities[id], (uint) component);
        int index = Array.IndexOf(Enum.GetValues(component.GetType()), component);
        ComponentManager.COMPONENTS[index].data[id] = null;
        ComponentManager.COMPONENTS[index].entities.Remove(id);
        if (EntityManager.entities[id] == (uint)COMPONENT.NONE) EntityManager.clear(id);

    }
    public static void unsubscribe(uint id, params COMPONENT[] components)
    {
        for (int i = 0; i < components.Length; i++)
        {
            unsubscribe(id, components[i]);
        }
        if (EntityManager.entities[id] == (uint) COMPONENT.NONE) EntityManager.clear(id);
    }



    public static bool has(uint id, params COMPONENT[] components)
    {

        uint bitset = EntityManager.entities[id];
        foreach (COMPONENT component in components)
        {
            if (!bitHas(bitset, (uint)component)) return false;
        }
        return true;
    }

    public static bool has(uint id, COMPONENT component)
    {
        return bitHas(EntityManager.entities[id], (uint)component);
    }

    public static bool bitHas(uint src, uint set)
    {
        return (src & set) >= 1;
    }

    public static uint bitSet(uint src, uint set)
    {
        return src | set;
    }

    public static uint bitDel(uint src, uint set)
    {
        uint mask = ~set;
        return src & mask;
    }
}