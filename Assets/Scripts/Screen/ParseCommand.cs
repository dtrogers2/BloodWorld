using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParseCommand
{
    public Creature player;
    public IGame game;
    public IScreenMaker maker;
    public ParseCommand(IGame game, IScreenMaker maker)
    {
        this.game = game;
        this.maker = maker;
        this.player = game.player;
    }

    public bool parseKeycodeTurn(KeyCode c, IStack ss, out float actionCost)
    {
        ICmd cmd = parseKey(c, ss);
        actionCost = 0;
        return (cmd != null) ? cmd.turn(out actionCost) : false;
    }

    public ICmd parseKey(KeyCode c, IStack ss)
    {
        IScreen s = null;
        Vector3Int dir = new Vector3Int();
        switch(c)
        {
            case KeyCode.LeftArrow: case KeyCode.A: dir.x -= 1; break;
            case KeyCode.RightArrow: case KeyCode.D: dir.x += 1; break;
            case KeyCode.UpArrow: case KeyCode.W: dir.y -= 1; break;
            case KeyCode.DownArrow: case KeyCode.S: dir.y += 1; break;
            case KeyCode.Home: dir.x -= 1; dir.y -= 1; break;
            case KeyCode.End: dir.x -= 1; dir.y += 1; break;
            case KeyCode.PageUp: dir.x += 1; dir.y -= 1; break;
            case KeyCode.PageDown: dir.x += 1; dir.y += 1; break;
            case KeyCode.Delete: case KeyCode.Period: case KeyCode.Clear: return this.waitCmd();
            case KeyCode.Q: s = new LogScreen(game, maker); break;
            default: return null;
        }

        if (s != null)
        {
            ss.push(s); return null;
        }

        if (dir != Vector3Int.zero)
        {
            return bumpCmd(dir);
        }
        return null;
    }

    public ICmd moveCmd(Vector3Int dir)
    {
        return new MoveCmd(dir, player, game);
    }

    public ICmd bumpCmd(Vector3Int dir)
    {
        return new BumpCmd(dir, player, game);
    }

    public ICmd waitCmd()
    {
        return new WaitCmd(player, game);
    }
}
