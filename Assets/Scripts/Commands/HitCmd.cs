using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Unity.Burst.Intrinsics.X86.Avx;

public class HitCmd : CmdBase
{
    public uint tgt { get; }
    public HitCmd(uint me, uint tgt, IGame game) : base(me, game)
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
        actionCost = 1.0f;
        if (ENTITY.has(me, COMPONENT.CREATURE))
        {
            Creature meC = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[me];
            actionCost = meC.attackRate;

        }
        return exc();
    }

    public void doDmg(int dmg, uint tgt, uint src, IGame game)
    {
        Creature cTgt;
        Creature cSrc;
        string s = "";
        if (dmg < 0) dmg = 0;
        if (ENTITY.has(src, COMPONENT.CREATURE))
        {
            cSrc = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[src];
            s += (dmg > 0) ? $"{cSrc.name}({src}) hits " : $"{cSrc.name}({src}) misses ";
        }
        if (ENTITY.has(tgt, COMPONENT.CREATURE))
        {
            cTgt = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[tgt];
            s += (dmg > 0) ? $"{cTgt.name}({tgt}) for {dmg} damage." : $"{cTgt}({tgt}).";
        }
        //TODO add Hit Dice component
        game.msg(s);
        if (dmg != 0) HealthAdj.adjust(tgt, -dmg, game, src);
    }


    public int calcDmg(Rng rng, uint src)
    {
        return 1;
    }
}
