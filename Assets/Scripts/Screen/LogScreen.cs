using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LogScreen : BaseScreen
{
    public string name { get; } = "log";
    public string[] msgLog;
    public LogScreen(IGame game, IScreenMaker maker) : base(game, maker)
    {
        this.msgLog = game.log.archive.ToArray();
    }
     override public void draw(ITerm term)
    {
        int range = term.dim.y - 1;
        if (msgLog.Length < range) range = msgLog.Length;

        for (int y = 0; y < range; y++)
        {
            string s = DrawScreen.extend(msgLog[y], term);
            term.txt(3, 1 + y, s, ColorHex.White, ColorHex.Black, " ");
        }
    }

    override public void onKey(KeyCode e, IStack stack)
    {
        stack.pop();
    }
}
