using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonCmd : CmdBase
{
    uint item;
    float cost = 0f;
    public DonCmd(uint me, uint item, IGame game) : base(me, game)
    {
        this.item = item;
    }

    public override bool exc()
    {
        if (!ENTITY.has(item, COMPONENT.ITEM)) return false;
        Item it = (Item)ComponentManager.get(COMPONENT.ITEM).data[item];
        bool equipped = ItemSystem.donItem(me, item, game, out cost);
        if (equipped)
        {
            if (me == game.playerId) game.msg(new Msg { text = $"Equipped {it.name};", color = COLOR.Green });
        }

        return equipped;
    }

    public override bool turn(out float actionCost)
    {
        bool exc = this.exc();
        actionCost = cost;

        return exc;
    }
}
