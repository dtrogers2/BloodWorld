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
        Position p = (Position)game.build.POSITIONS.data[me];
        Vector3Int tgtPos = new Vector3Int(p.x + dir.x, p.y + dir.y, p.z + dir.z);
        /*
        if (game.world.getTile(tgtPos, game, out Tile tile))
        {
            ICmd cmd = (tile.creature != null) ? new HitCmd(me, tile.creature, game) : new MoveCmd(dir, me, game);
            return cmd.turn(out actionCost);
        }*/

            // Search through nearby entities to see if there are valid targets to attack at target position
            List<uint> targets = new List<uint>();
            Stack<IRegion> regions = game.world.getRegions(tgtPos, 0, game);
            while (regions.Count > 0)
            {
                IRegion r = regions.Pop();
                for (int i = 0; i < r.entities.Count; i++)
                {
                    uint eId = r.entities.ElementAt(i);
                    if (ENTITY.has(eId,game.build.POSITIONS ))
                    {
                        Position otherP = (Position) game.build.POSITIONS.data[eId];

                        if (otherP.x == tgtPos.x && otherP.y == tgtPos.y)
                        {
                           targets.Add(eId);
                        }
                    }
                    //if (!activeCreatures.Contains(r.creatureList[i]) && r.creatureList[i] != game.player) activeCreatures.Push(r.creatureList[i]);
                }
            
            // Filter targets list for best target
            ICmd cmd = (targets.Count > 0) ? new HitCmd(me, targets[0], game) : new MoveCmd(dir, me, game);
            return cmd.turn(out actionCost);
        }

        return this.exc();
    }
}
