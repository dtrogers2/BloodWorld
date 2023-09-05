using System.Collections;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using UnityEngine;

public class LogScreen : BaseScreen
{
    public string name { get; } = "log";
    public Msglist[] msgLog;
    public LogScreen(IGame game, IScreenMaker maker) : base(game, maker)
    {
        this.msgLog = game.log.archive.ToArray();
    }
     override public void draw(ITerm term)
    {
        int range = term.dim.y - 1;
        int trueLength = 0;
        //if (msgLog.Length < range) range = msgLog.Length;
        for(int i = 0; i < msgLog.Length; i++)
        {
            if (msgLog[i].msgs.Count > 0) trueLength++;
        }
        if (trueLength < range) range = trueLength;
        //for (int y = 0; y < range; y++)
        //{
        //    string s = DrawScreen.extend(msgLog[y], term);
        //    term.txt(0, 1 + y, s, ColorHex.White, ColorHex.Black);
        //}
        int cursorX = 0;
        for (int i = msgLog.Length - 1, y = range; y >= 0 && i >= 0; y--, i--)
        {
            for (int j = 0; j < msgLog[i].msgs.Count; j++)
            {
                string baseS = (j == 0 ? "_" : "") + msgLog[i].msgs[j].text;
                string s = DrawScreen.extend(baseS, term);
                term.txt(cursorX, y, s, msgLog[i].msgs[j].color, ColorHex.Black);
                cursorX += baseS.Length;
            }
            cursorX = 0;
        }
    }

    override public void onKey(KeyCode e, IStack stack)
    {
        stack.pop();
    }
}
