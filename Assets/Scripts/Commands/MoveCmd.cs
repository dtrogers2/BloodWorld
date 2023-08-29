using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCmd : CmdBase
{
    Vector3Int dir;
    public MoveCmd(Vector3Int dir, uint me, IGame game) : base(me, game)
    {
        this.dir = dir;
    }

    public override bool exc()
    {
        Position p = (Position) ComponentManager.get(COMPONENT.POSITION).data[me];
        Vector3Int newPos = new Vector3Int(p.x + dir.x, p.y + dir.y, p.z + dir.z);
        bool legal = game.world.moveEntity(me, newPos, game);
        return legal;
    }

    public override bool turn(out float actionCost)
    {
        actionCost = 1.0f;
        return this.exc();
    }


}
