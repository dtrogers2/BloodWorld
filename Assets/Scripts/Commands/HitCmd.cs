using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class HitCmd : CmdBase
{
    public Creature tgt { get; }
    public HitCmd(Creature me, Creature tgt, IGame game) : base(me, game)
    {
        this.tgt = tgt;
    }

    public override bool exc()
    {
        int dmg = calcDmg(game.rng, me);
        doDmg(dmg, tgt, me, game);
        return true;
    }

    public override bool turn(out float actionCost)
    {
        actionCost = me.baseAtkCost;
        return exc();
    }

    public void doDmg(int dmg, Creature tgt, Creature src, IGame game)
    {
        string s = (dmg > 0) ? $"{src.name} hits {tgt.name}" : $"{src.name} misses {tgt.name}";
        tgt.hp -= dmg;
        if (tgt.hp < 1) mobDies(tgt, game);
        game.msg(s);
    }

    public void mobDies(Creature m, IGame game)
    {
        string s = $"{m.name} dies";
        game.msg(s);
        if (m !=  game.player) {
            game.world.removeEntity(m, game);
        }
    }

    public int calcDmg(Rng rng, Creature src)
    {
        return 1;
    }
}
