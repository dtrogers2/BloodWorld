using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;

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
        return ++curLength;
    }

    public static void reset()
    {
        entities = new uint[ENTITIES_DEFAULT];
        curLength = 0;
    }
}


public struct ENTITY
{
    public static void subscribe(uint id, object data, ComponentInf component)
    {
        // TODO try simpler version
        //component.data[data);
        uint eBits = EntityManager.entities[id];
        string cName = component.GetType().Name.ToUpper().Replace("COMPONENT", "");
        if (Enum.TryParse(cName, out COMPONENT c))
        {
            uint newSet = bitSet(eBits, (uint)c);
            EntityManager.entities[id] = newSet;
            component.data[id] =  data;
            component.entities.Add(id);
        }
        else
        {
            throw new Exception($"Entity {id} has a component that could not be parsed!!");
        }
    }
    public static void subscribe(uint id, object[] data, params ComponentInf[] components)
    {
        int paramIx = 0;
        foreach (ComponentInf component in components)
        {
            // TODO try simpler version
            //component.data[data);
            uint eBits = EntityManager.entities[id];
            string cName = component.GetType().Name.ToUpper().Replace("COMPONENT", "");
            if (Enum.TryParse(cName, out COMPONENT c))
            {
                uint newSet = bitSet(eBits, (uint)c);
                EntityManager.entities[id] = newSet;

                component.data[id] =  data[paramIx];
                component.entities.Add(id);
            }
            else
            {
                throw new Exception($"Entity {id} has a component that could not be parsed!!");
            }
            paramIx++;
        }
    }

    public static void unsubscribe(uint id, params ComponentInf[] components)
    {
        foreach (ComponentInf component in components)
        {
            string cName = component.GetType().Name.ToUpper().Replace("COMPONENT", "");
            if (Enum.TryParse(cName, out COMPONENT c))
            {
                EntityManager.entities[id] = bitDel(EntityManager.entities[id], (uint)c);

                component.data[id] = null;
                component.entities.Remove(id);
            }
            else
            {
                throw new Exception($"Entity {id} has a component that could not be parsed!!");
            }
        }
        if (EntityManager.entities[id] == (uint) COMPONENT.NONE) EntityManager.clear(id);
    }

    public static void unsubscribe(uint id, ComponentInf component)
    {
        string cName = component.GetType().Name.ToUpper().Replace("COMPONENT", "");
        if (Enum.TryParse(cName, out COMPONENT c))
        {
            EntityManager.entities[id] = bitDel(EntityManager.entities[id], (uint)c);
            component.data[id] = null;
            component.entities.Remove(id);
        }
        else
        {
            throw new Exception($"Entity {id} has a component that could not be parsed!!");
        }
        if (EntityManager.entities[id] == (uint) COMPONENT.NONE) EntityManager.clear(id);

    }

    public static bool has(uint id, params ComponentInf[] components)
    {

        uint bitset = EntityManager.entities[id];
        foreach (ComponentInf component in components)
        {
            // Test simpler version
            // return !component.data.Contains(id);
            string cName = component.GetType().Name.ToUpper().Replace("COMPONENT", "");
            if (Enum.TryParse(cName, out COMPONENT c)) {
                if (!bitHas(bitset, (uint) c)) return false;
            } else
            {
                throw new Exception($"Entity {id} has a component that could not be parsed!!");
            }
        }
        return true;
    }

    public static bool has(uint id, ComponentInf component)
    {
        // return !component.data.Contains(id);
        string cName = component.GetType().Name.ToUpper().Replace("COMPONENT", "");
        if (Enum.TryParse(cName, out COMPONENT c))
        {
            return bitHas(EntityManager.entities[id], (uint)c);
        }
        else
        {
            throw new Exception($"Entity {id} has a component that could not be parsed!!");
        }
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
