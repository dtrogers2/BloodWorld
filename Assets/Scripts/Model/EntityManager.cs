using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.Timeline;

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
    public static void subscribe(uint id, object[] data)
    {
        for (int i = 0; i < data.Length; i++)
        {
            subscribe(id, data[i]);
        }
    }

    public static void subscribe(uint id, object data)
    {
        string componentName = data.GetType().Name.ToUpper();
        COMPONENT component = (COMPONENT) Enum.Parse(typeof(COMPONENT), componentName);
        uint eBits = EntityManager.entities[id];
        uint newSet = bitSet(eBits, (uint)component);
        EntityManager.entities[id] = newSet;
        int index = Array.IndexOf(Enum.GetValues(component.GetType()), component);
        ComponentManager.COMPONENTS[index].data[id] = data;
        ComponentManager.COMPONENTS[index].entities.Add(id);
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
    }

    public static uint get(uint id)
    {
        return EntityManager.get(id);
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

    public static bool bitIs(uint src, uint set)
    {
        uint mask = ~src;
        return ~(set ^ mask) == 0;
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