using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BumpCmd : CmdBase
{
    public Vector3Int dir;
    ICmd cmd;
    public BumpCmd(Vector3Int dir, Creature me, IGame game) : base(me, game)
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
        actionCost = me.baseMoveCost;
        Vector3Int tgtPos = me.position + dir;
        if (game.world.getTile(tgtPos, game, out Tile tile))
        {
            ICmd cmd = (tile.creature != null) ? new HitCmd(me, tile.creature, game) : new MoveCmd(dir, me, game);
            return cmd.turn(out actionCost);
        }

        return this.exc();
    }
}
