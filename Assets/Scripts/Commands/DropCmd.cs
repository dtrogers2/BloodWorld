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
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        bool dropped = ItemSystem.dropItem(me, item, game);
        if (dropped)
        {
            cost = 1f;
            if (me == game.playerId) game.msg(new Msg { text = $"Dropped {it.name};", color = COLOR.Yellow });
        }
        
        return dropped;
    }

    public override bool turn(out float actionCost)
    {
        bool exc = this.exc();
        actionCost = cost;

        return exc;
    }
}
