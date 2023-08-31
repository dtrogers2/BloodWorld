using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BumpCmd : CmdBase
{
    public Vector3Int dir;
    ICmd cmd;
    public BumpCmd(Vector3Int dir, uint me, IGame game) : base(me, game)
    {
        this.dir = dir;
    }

    public override bool exc()
    {
        //Vector3Int tgtPos = me.position + dir;
        //IRegion region = game.world.getRegion(me.position, game);
        //Vector2Int lclPos = (Vector2Int) game.world.localizePosition(me.position);
        //if (!region.legal(lclPos.x, lclPos.y)) return false;
        //if (game.world.getTile(tgtPos, game, out Tile tile))
        //{
        //    ICmd cmd = (tile.creature != null) ? new HitCmd(me, tile.creature, game) : new MoveCmd(dir, me, game);
        //    return cmd.turn(out float actionCost);
        //}
        return false;
        
    }

    public override bool turn(out float actionCost)
    {
        actionCost = 1.0f;
        Position p = (Position) ComponentManager.get(COMPONENT.POSITION).data[me];
        Vector3Int tgtPos = new Vector3Int(p.x + dir.x, p.y + dir.y, p.z + dir.z);
        // Search through nearby entities to see if there are valid targets to attack at target position

        uint tgtId = 0;
        bool tgtCreature = false;
        if (game.world.getCellFlags(tgtPos, game, out uint flags))
        {
            if (ENTITY.bitHas(flags, (uint)CELLFLAG.CREATURE))
            {
                tgtCreature = game.world.getCellEntity(tgtPos, game, out tgtId);
            }

            ICmd cmd = (tgtCreature) ? new HitCmd(me, tgtId, game) : new MoveCmd(dir, me, game);
            return cmd.turn(out actionCost);
        }

        return this.exc();
    }
}
