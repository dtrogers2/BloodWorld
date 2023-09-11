using System;
using System.Collections.Generic;

[Serializable]
public class Inventory : Component
{
    public ITEMFLAG allowedItems = ITEMFLAG.ALL;
    public List<uint> items = new List<uint>();
}
