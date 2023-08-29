using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CmdBase : ICmd
{
    public uint me { get; }

    public IGame game { get; }
    public CmdBase(uint me, IGame game)
    {
        this.me = me;
        this.game = game;
    }


    public virtual bool exc()
    {
        throw new System.NotImplementedException();
    }

    public bool raw()
    {
        return this.exc();
    }

    public ICmd setDir(Vector2Int dir)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool turn(out float actionCost)
    {
        actionCost = 1.0f;
        return this.exc();
    }
}
