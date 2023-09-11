
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEditor.Progress;

public class GetCmd : CmdBase
{

    uint item;
    float cost = 0f;
    public GetCmd(uint me, uint item, IGame game) : base(me, game) {
        this.item = item;
    }

    public override bool exc()
    {
        if (!ENTITY.has(me, COMPONENT.INVENTORY) || !ENTITY.has(item, COMPONENT.ITEM) || !ENTITY.has(me, COMPONENT.POSITION) || !ENTITY.has(item, COMPONENT.POSITION)) return false;
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[me];
        Position p2 = (Position)ComponentManager.get(COMPONENT.POSITION).data[item];
        if (p1.x != p2.x || p1.y != p2.y || p1.z != p2.z) return false;
        Inventory inv = (Inventory)ComponentManager.get(COMPONENT.INVENTORY).data[me];
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
        cost = 1f;
        if (me == game.playerId) game.msg(new Msg { text = $"Acquired {it.name};", color = COLOR.Yellow});
        handleOwnership(it);
        return true;
    }

    public override bool turn(out float actionCost)
    {
        bool exc = this.exc();
        actionCost = cost;

        return exc;
    }

    public void handleOwnership(Item it)
    {
        if (it.owner == 0) it.owner = me;
        // If the player is not friendly towards the owner, then the owner is aggroed
    }
}
