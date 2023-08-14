using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseScreen : IScreen
{
    public string name { get; set; } = "Based";
    public IGame game;
    public IScreenMaker maker;

    public BaseScreen(IGame game, IScreenMaker maker)
    {
        this.game = game;
        this.maker = maker;
    }

    public void draw(ITerm term)
    {
        DrawScreen.drawMapPlayer(term, game.player.position, game);
        DrawScreen.renderHUD(term, game);
        DrawScreen.renderMsgs(term, game);
    }

    public void npcTurns(IStack stack, float actionCost)
    {
        Creature player = game.player;
        IRegion map = game.curMap();
        TurnQueue q = map.queue;
        for(Creature m = q.next(); m != player && !this.over(stack); m = q.next())
        {
            npcTurn(m, player);
        }
        handleMsgs(stack);
    }
    public void npcTurn(Creature me, Creature tgt)
    {
        
        me.actionCost -= tgt.actionCost;
        IAI ai = game.ai;
        if (ai != null) 
        {
            float newCost = 0.0f;
            ai.turn(me, tgt, game, out newCost);
            me.actionCost += newCost;
        }
    }
    public void handleMsgs(IStack s)
    {
        if (game.log == null) return;
        if (game.log.queuedMsgs())
        {
            s.push(maker.more(game));
        }
    }

    public bool over(IStack s)
    {
        bool over = !game.player.alive();
        if (over)
        {
            s.pop();
            s.push(maker.gameOver());
        }

        return over;
    }

    public virtual void onKey(KeyCode keycode, IStack stack)
    {
        Debug.Log("BaseScreen onKey");
    }

    public void onKey(KeyCode keycode)
    {
        throw new System.NotImplementedException();
    }
}
