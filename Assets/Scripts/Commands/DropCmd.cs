using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropCmd : CmdBase
{
    uint item;
    float cost = 0f;
    public DropCmd(uint me, uint item, IGame game) : base(me, game)
    {
        this.item = item;
    }

    public override bool exc()
    {
        if (!ENTITY.has(me, COMPONENT.INVENTORY) || !ENTITY.has(item, COMPONENT.ITEM) || !ENTITY.has(me, COMPONENT.POSITION)) return false;
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[me];
        Inventory inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[me];
        if (!inv.items.Contains(item)) return false;
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        if (it.equipped) {game.msg(new Msg { text = $"{it.name} is equipped!", color = COLOR.GrayDark}); return false; }
        Position p = new Position { x = p1.x, y = p1.y, z = p1.z };
        ENTITY.subscribe(item, p);
        game.world.addEntity(item, game);
        if (ENTITY.has(me, COMPONENT.CREATURE)) {
            // Set the items cellstack to the current mobs cellstack child
            CellStack itemStack = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[item];
            CellStack meStack = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[me];
            itemStack.entity = meStack.entity;
            // If the acting entity is a creature, set it to the cell entity
            game.world.setCellEntity(new Vector3Int(p1.x, p1.y, p1.z), game, me);
            // Set the acting entities cellstack child to the item
            meStack.entity = item;
        }
        cost = 1f;
        inv.items.Remove(item);
        if (me == game.playerId) game.msg(new Msg { text = $"Dropped {it.name};", color = COLOR.Yellow });
        return true;
    }

    public override bool turn(out float actionCost)
    {
        bool exc = this.exc();
        actionCost = cost;

        return exc;
    }
}
