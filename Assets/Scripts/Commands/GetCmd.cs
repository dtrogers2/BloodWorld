
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
        if (!ENTITY.has(item, COMPONENT.ITEM)) return false;
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        bool got = ItemSystem.getItem(me, item, game);
        if (got)
        {
            cost = 1f;
            if (me == game.playerId) game.msg(new Msg { text = $"Acquired {it.name};", color = COLOR.Yellow });
            handleOwnership(it);
        }
        return got;
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
