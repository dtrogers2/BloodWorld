using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    public List<Item> inventory = new List<Item>();
    public int len() { return inventory.Count; }
    public bool tryAdd(Item i)
    {
        if (len() >= 26 ) return false;
        inventory.Add(i);
        return true;
    }

    public bool tryDrop(int index)
    {
        if (index < 0 || index >= len()) return false;
        Item i = inventory[index];
        if (i.tryRemove())
        {
            inventory.RemoveAt(index);
            return true;
        }
        return false;
    }
}
