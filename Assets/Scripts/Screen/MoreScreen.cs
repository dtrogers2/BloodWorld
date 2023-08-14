using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreScreen : BaseScreen
{
    public string name { get; } = "more";
    public MoreScreen(IGame game, IScreenMaker maker) : base(game, maker) {}
    public new void onKey(KeyCode e, IStack stack)
    {
        MsgLog log = this.game.log;
        log.dequeue();
        if (!log.queuedMsgs()) { stack.pop(); }
    }
}
