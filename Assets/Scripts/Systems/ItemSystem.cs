
using System;
using System.Collections.Generic;
using UnityEngine;

public static class ItemSystem
{
    public static bool getItem(uint actor, uint item, IGame game)
    {
        if (!ENTITY.has(actor, COMPONENT.INVENTORY) || !ENTITY.has(item, COMPONENT.ITEM) || !ENTITY.has(actor, COMPONENT.POSITION) || !ENTITY.has(item, COMPONENT.POSITION)) return false;
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[actor];
        Position p2 = (Position)ComponentManager.get(COMPONENT.POSITION).data[item];
        if (p1.x != p2.x || p1.y != p2.y || p1.z != p2.z) return false;
        Inventory inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[actor];
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        bool itemValid = true;
        for (int i = 0; i < Enum.GetNames(typeof(ITEMFLAG)).Length - 1; i++)
        {
            if (ENTITY.bitHas((uint)it.flags, (uint)(1 << i)) && !ENTITY.bitHas((uint)inv.allowedItems, (uint)(1 << i))) itemValid = false;
        }
        if (!itemValid) return false;
        inv.items.Add(item);
        game.world.removeEntity(item, game);
        ENTITY.unsubscribe(item, COMPONENT.POSITION);
        return true;
    }

    public static List<uint> getItemsAt(uint entity, IGame game)
    {
        List<uint> items = new List<uint>();
        if (!ENTITY.has(entity, COMPONENT.POSITION)) return items;
        Position p = (Position)ComponentManager.get(COMPONENT.POSITION).data[entity];

        if (game.world.getCellStack(new Vector3Int(p.x, p.y, p.z), game, out Stack<uint> stack))
        {
            for (int i = stack.Count; i > 0; i--)
            {
                uint e = stack.Pop();
                if (ENTITY.has(e, COMPONENT.ITEM)) items.Add(e);
            }
        }
        return items;
    }

    public static bool dropItem(uint owner, uint item, IGame game, out float delay)
    {
        delay = 0f;
        if (!ENTITY.has(owner, COMPONENT.INVENTORY) || !ENTITY.has(item, COMPONENT.ITEM) || !ENTITY.has(owner, COMPONENT.POSITION)) return false;
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[owner];
        Inventory inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[owner];
        if (!inv.items.Contains(item)) return false;
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        if (it.equipped) { 
            if (!doffItem(owner, item, game, out delay)) return false; 
        }
        Position p = new Position { x = p1.x, y = p1.y, z = p1.z };
        ENTITY.subscribe(item, p);
        game.world.addEntity(item, game);
        if (ENTITY.has(owner, COMPONENT.CREATURE))
        {
            // Set the items cellstack to the current mobs cellstack child
            CellStack itemStack = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[item];
            CellStack meStack = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[owner];
            itemStack.entity = meStack.entity;
            // If the acting entity is a creature, set it to the cell entity
            game.world.setCellEntity(new Vector3Int(p1.x, p1.y, p1.z), game, owner);
            // Set the acting entities cellstack child to the item
            meStack.entity = item;
        }
        inv.items.Remove(item);
        return true;
    }

    public static bool doffItem(uint entity, uint item, IGame game, out float delay)
    {
        delay = 0f;
        if (!ENTITY.has(entity, COMPONENT.INVENTORY) || !ENTITY.has(item, COMPONENT.ITEM)) return false;
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        if (!it.equipped) return true;
        Inventory inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[entity];
        if (!inv.items.Contains(item)) return false;
        if (ENTITY.bitHas((uint) it.flags, (uint) ITEMFLAG.CURSED))
        {
            if (game.playerId == entity)
            {
                game.msg(new Msg { text = $"{it.name} can't be removed!", color = COLOR.Red });
            }
            return false;
        }
        delay = (ENTITY.bitHas((uint) it.equipslot, (uint) EQUIPSLOT.RING) || (ENTITY.bitHas((uint)it.equipslot, (uint)EQUIPSLOT.NECKLACE))) 
            ? .3f : (ENTITY.bitHas((uint)it.equipslot, (uint)EQUIPSLOT.ARMOR)) 
            ? 5f : (ENTITY.bitHas((uint)it.equipslot, (uint)EQUIPSLOT.QUIVER)) ? 0f : 1f;
        if (game.playerId == entity)
        {
            game.msg(new Msg { text = $"Removed {it.name}.", color = COLOR.Red });
        }
        it.equipped = false;
        return true;
    }
    public static bool donItem(uint entity, uint item, IGame game, out float delay)
    {
        delay = 0f;
        if (!ENTITY.has(entity, COMPONENT.INVENTORY) || !ENTITY.has(item, COMPONENT.ITEM)) return false;
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        Inventory inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[entity];
        uint handEty = 0;
        uint ringEty = 0;
        for (int i = 0; i < inv.items.Count; i++)
            {
                uint invItem = inv.items[i];
                Item it2 = (Item)ComponentManager.get(COMPONENT.ITEM).data[invItem];
                if (!it2.equipped) continue;
                if (ENTITY.bitHas((uint) it.equipslot, (uint) EQUIPSLOT.TWOHANDED)  && (ENTITY.bitHas((uint)it2.equipslot, (uint)EQUIPSLOT.TWOHANDED) 
                || ENTITY.bitHas((uint)it2.equipslot, (uint)EQUIPSLOT.HAND))) // If the equipped item occupies hands
                {
                        if (!doffItem(entity, invItem, game, out float tmp)) return false;
                        delay += tmp;
                        continue;
                } 
                else if (ENTITY.bitIs((uint) it.equipslot, (uint) EQUIPSLOT.HAND) && ENTITY.bitIs((uint)it2.equipslot, (uint)EQUIPSLOT.HAND)) //If a 1 handed item is equipped
                {
                    if (handEty > 0)
                    {
                        // Unequip the 2nd ring;
                        if (!doffItem(entity, invItem, game, out float tmp)) return false;
                        delay += tmp;
                        break;
                    }
                    else
                    {
                        handEty = invItem;
                        continue;
                    }
                } 
                else if (ENTITY.bitIs((uint)it.equipslot, (uint) EQUIPSLOT.RING) && ENTITY.bitIs((uint) it2.equipslot, (uint) EQUIPSLOT.RING)) // If a ring is equipped, 
                {
                    if (ringEty > 0) {
                        // Unequip the 2nd ring;
                        if (!doffItem(entity, invItem, game, out float tmp)) return false;
                        delay += tmp;
                        break;
                    }
                    else
                    {
                        ringEty = invItem;
                        continue;
                    }
                } 
                else if (ENTITY.bitHas((uint)it2.equipslot, (uint) it.equipslot))
                {
                    if (!doffItem(entity, invItem, game, out float tmp)) return false;
                    delay += tmp;
                }
        }
        it.equipped = true;
        return true;
    }
}
