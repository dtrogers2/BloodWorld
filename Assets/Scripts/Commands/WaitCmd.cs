using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitCmd : CmdBase
{
    public WaitCmd(Creature me, IGame game) : base(me, game)
    {

    }

    override public  bool exc()
    {
        return true;
    }
}
