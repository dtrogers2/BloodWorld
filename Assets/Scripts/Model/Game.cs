using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public interface IGame
{
    public Rng rng { get; set; }
    public uint playerId { get; set; }
    public IAI ai { get; set; }
    public void msg(Msg s);
    public void flash(Msg s);
    public MsgLog log { get; }

    public World world { get; set; } 

    public IBuild build { get; set; }
    public float time { get; set; }


}

public class Game : IGame
{
    public Rng rng { get ; set ; }
    public uint playerId { get; set; }
    public MsgLog log { get; set; } = new MsgLog();
    public float time { get; set; } = 0f;
    public World world { get; set; } = new World(new Vector3Int(3, 3, 1));

    public IAI ai { get; set; }
    public IBuild build { get ; set ; }

    public Game(Rng rng, IBuild build)
    {
        this.rng = rng;
        this.build = build;

    }

    public void msg(Msg s)
    {
        this.log.msg(s, time, false);
    }

    public void flash(Msg s)
    {
        this.log.msg(s, time, true);
    }
} 