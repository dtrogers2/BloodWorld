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

    virtual public void draw(ITerm term)
    {
        DrawScreen.drawMapPlayer(term, game.player.position, game);
        DrawScreen.renderHUD(term, game);
        DrawScreen.renderMsgs(term, game);
    }

    public void npcTurns(IStack stack, float actionCost)
    {
        Creature player = game.player;
        Stack<Creature> activeCreatures = new Stack<Creature>();
        Stack<IRegion> regions = game.world.getRegions(player.position, 1, game);
        while (regions.Count > 0)
        {
            IRegion r = regions.Pop();
            for (int i = 0; i < r.creatureList.Count; i++)
            {
                if (!activeCreatures.Contains(r.creatureList[i]) && r.creatureList[i] != game.player) activeCreatures.Push(r.creatureList[i]);
            }
        }
        for (int i = activeCreatures.Count; i > 0; i--)
        {
            npcTurn(activeCreatures.Pop(), game.player);
        }
        handleMsgs(stack);
        finishPlayerTurn();
    }
    public void npcTurn(Creature me, Creature tgt)
    {
        
        me.actionCost -= tgt.actionCost;
        IAI ai = game.ai;
        if (ai != null && me.actionCost <= 0) 
        {
            while (me.actionCost <= 0)
            {
                float newCost = 0.0f;
                ai.turn(me, tgt, game, out newCost);
                me.actionCost += newCost;
            }
        }
    }

    public void finishTurn(Creature c)
    {

    }

    public void finishPlayerTurn()
    {
        game.player.actionCost = 0f;
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
