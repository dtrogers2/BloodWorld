using System;
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
        bool hit = calcHit(game.rng, tgt);
        int dmg = calcDmg(game.rng);
        
        doDmg(dmg, hit, tgt, game);
        return true;
    }

    public override bool turn(out float actionCost)
    {
        // Choose an attack here?
        actionCost = 1.0f;
        return exc();
    }

    public void doDmg(int dmg, bool hit, uint tgt, IGame game)
    {
        Creature cTgt;
        Creature cSrc;
        string s = "";
        if (dmg < 0) dmg = 0;
        if (ENTITY.has(me, COMPONENT.CREATURE))
        {
            cSrc = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[me];
            s += (dmg > 0 && hit) ? $"{cSrc.name}({me}) hits " : $"{cSrc.name}({me}) misses ";
        }
        if (ENTITY.has(tgt, COMPONENT.CREATURE))
        {
            cTgt = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[tgt];
            s += (dmg > 0 && hit) ? $"{cTgt.name}({tgt}) for {dmg} damage." : $"{cTgt.name}({tgt}).";
        }
        //TODO add Hit Dice component
        Msg m = new Msg { color = (dmg > 0 && hit ) ? COLOR.White: COLOR.GrayDark, text = s};
        game.msg(m);
        if (dmg > 0 && hit) HealthAdj.adjust(tgt, -dmg, game, me);
    }

    public bool calcHit(Rng rng, uint tgt)
    {
        int toHit = -1;
        int tgtAC = 11;
        if (ENTITY.has(me, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[me];
            //if (c.levels[1] / 2 > toHit) toHit = c.levels[1] / 2; // MONSTER
            //if (c.levels[2] / 3 > toHit) toHit = c.levels[2] / 3; // FIGHTER
            //if (c.levels[3] / 3 > toHit) toHit = c.levels[3] / 5; // MAGICIAN
            //if (c.levels[4] / 3 > toHit) toHit = c.levels[4] / 4; // CLERIC
            //if (c.levels[5] / 3 > toHit) toHit = c.levels[5] / 4; // THIEF
            //if (c.levels[6] / 3 > toHit) toHit = c.levels[6] / 3; // ELF
            //if (c.levels[7] / 3 > toHit) toHit = c.levels[7] / 3; // DWARF
            //if (c.levels[8] / 3 > toHit) toHit = c.levels[8] / 3; // HALFLING
        }
        if (ENTITY.has(tgt, COMPONENT.DEFENSES)) {
            Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[tgt];
            tgtAC = d.AC;
        }
        return (rng.roll("1d20") + toHit >= tgtAC);
    }

    public int calcDmg(Rng rng)
    {
        int dmg = 0;
        if (ENTITY.has(me, COMPONENT.ATTACKS)) {
            Attacks a = (Attacks) ComponentManager.get(COMPONENT.ATTACKS).data[me];
            if (a.attacks.Length > 0)
                dmg = rng.roll(a.attacks[0].dmgDice);
        }
        return dmg;
    }
}
