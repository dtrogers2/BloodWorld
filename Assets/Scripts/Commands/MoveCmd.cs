using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCmd : CmdBase
{
    Vector3Int dir;
    public MoveCmd(Vector3Int dir, Creature me, IGame game) : base(me, game)
    {
        this.dir = dir;
    }

    public override bool exc()
    {
        Vector3Int newPos = me.position + dir;
        bool legal = game.world.moveEntity(me, newPos, game);
        return legal;
    }

    public override bool turn(out float actionCost)
    {
        actionCost = me.baseMoveCost;
        return this.exc();
    }


}
