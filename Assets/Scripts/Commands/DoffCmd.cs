using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoffCmd : CmdBase
{
    uint item;
    float cost = 0f;
    public DoffCmd(uint me, uint item, IGame game) : base(me, game)
    {
        this.item = item;
    }

    public override bool exc()
    {
        if (!ENTITY.has(item, COMPONENT.ITEM)) return false;
        bool unequipped = ItemSystem.doffItem(me, item, game, out cost);
        return unequipped;
    }

    public override bool turn(out float actionCost)
    {
        bool exc = this.exc();
        actionCost = cost;

        return exc;
    }
}
