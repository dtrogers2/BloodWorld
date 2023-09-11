using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptScreen : BaseScreen
{
    public string name { get; set; } = "optionscreen";
    public bool[][] options;
    public bool wrapX;
    public bool wrapY;
    public int curX = 0;
    public int curY = 0;
    public OptScreen(IGame game, IScreenMaker m, bool[][] options, bool wrapX = false, bool wrapY = false) : base(game, m)
    {
        this.options = options;
        this.wrapX = wrapX;
        this.wrapY = wrapY;
    }

    override public void draw(ITerm term)
    {
        throw new NotImplementedException();
    }

    override public void onKey(KeyCode keycode, IStack stack)
    {
        Vector2Int dir = Vector2Int.zero;
        switch (keycode)
        {
            case KeyCode.LeftArrow: case KeyCode.A: dir.x = -1; break;
            case KeyCode.RightArrow: case KeyCode.D: dir.x = 1; break;
            case KeyCode.UpArrow: case KeyCode.W: dir.y = -1; break;
            case KeyCode.DownArrow: case KeyCode.S: dir.y = 1; break;
            case KeyCode.Home: dir.x = -1; dir.y = -1; break;
            case KeyCode.End: dir.x = -1; dir.y = 1; break;
            case KeyCode.PageUp: dir.x = 1; dir.y = -1; break;
            case KeyCode.PageDown: dir.x = 1; dir.y = 1; break;
            //case KeyCode.Return: options[curY][curX] = !options[curY][curX]; break;
            case KeyCode.Escape:
                stack.pop();
                break;
            default: break;
        }
        if (dir != Vector2Int.zero) moveCursor(dir);
    }


    public void moveCursor(Vector2Int dir)
    {
        
        if (dir.x == -1 )
        {
            if (curX > 0) curX--;
        }

        if (dir.x == 1)
        {
            if (options.Length > 0)
            {
                if (curX + 1 < options[curY].Length) curX++;
            }
        }

        if (dir.y == -1 )
        {
            if (curY > 0) curY--;
        }

        if (dir.y == 1)
        {
            if (curY + 1 < options.Length) curY++;
        }
    }

    public char pos2char(int pos)
    {
        return (char)(pos + 65);
    }

    public int char2pos(char c)
    {
        int pos = (int)c - (int)'a';
        if (pos < 0 || pos >= options.Length) pos = -1;
        return pos;
    }
}
