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
        Vector3Int oldPos = new Vector3Int(p.x, p.y, p.z);
        Vector3Int newPos = new Vector3Int(p.x + dir.x, p.y + dir.y, p.z + dir.z);
        bool legal = game.world.moveEntity(me, newPos, game);
        if (legal)
        {
            // Leave stain at old position
            if (ENTITY.has(me, COMPONENT.STAIN))
                StainSystem.leaveStains(me, game, oldPos);
            //Get Stain at new position
            StainSystem.gainStains(me, game, newPos);
        }
        return legal;
    }

    public override bool turn(out float actionCost)
    {
        actionCost = 1.0f;
        if (ENTITY.has(me, COMPONENT.CREATURE))
        {
            Creature meC = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[me];
            actionCost = 30 / meC.moveSpeed;

        }

        return this.exc();
    }


}
