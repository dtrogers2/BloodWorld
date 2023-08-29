using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Item : IEntity
{
    public ItemSlot Slot { get; }
    public ItemSlot equippedSlot { get; set; }
    public bool tryRemove();
    public bool tryDrop();
    public bool tryWear();
    public bool tryUse();
}

public enum ItemSlot
{
    None = 0,
    Head = 1,
    Face = 2,
    Body = 4,
    Back = 8,
    ArmRight = 16,
    ArmLeft = 32,
    Legs = 64,
}
