using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        Vector3Int pos = Vector3Int.zero;
        if (ENTITY.has(game.playerId, COMPONENT.POSITION))
        {
            Position p = (Position)ComponentManager.get(COMPONENT.POSITION).data[game.playerId];
            pos = new Vector3Int(p.x, p.y, p.z);
            DrawScreen.drawMapPlayer(term, pos, game);
            DrawScreen.renderHUD(term, game);
            DrawScreen.renderMsgs(term, game);
        }

    }

    public void npcTurns(IStack stack)
    {
        Position p = (Position) ComponentManager.get(COMPONENT.POSITION).data[game.playerId];
        Vector3Int pos = new Vector3Int(p.x, p.y, p.z);
        Stack<uint> activeCreatures = new Stack<uint>();
        List<uint> nearCreatures = new List<uint>();
        Stack<IRegion> regions = game.world.getRegions(pos, 1, game);
        while (regions.Count > 0)
        {
            IRegion r = regions.Pop();
            for (int i = 0; i < r.entities.Count; i++)
            {
                if (r.entities.ElementAt(i) != game.playerId && !activeCreatures.Contains(r.entities.ElementAt(i)) && ENTITY.has(r.entities.ElementAt(i), COMPONENT.CREATURE)) {
                    activeCreatures.Push(r.entities.ElementAt(i));
                    nearCreatures.Add(r.entities.ElementAt(i));
                } 
            }
        }
        for (int i = activeCreatures.Count; i > 0; i--)
        {
            //insert logic to find nearest active creature it cares about

            npcTurn(activeCreatures.Pop(), nearCreatures);
        }
        handleMsgs(stack);
        finishPlayerTurn(stack);
    }
    public void npcTurn(uint me, List<uint> nearCreatures)
    {
        float playerCost = 0f;
        if (ENTITY.has(game.playerId, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[game.playerId];
            playerCost = c.AP;
        }
        if (ENTITY.has(me, COMPONENT.CREATURE) && ENTITY.has(me, COMPONENT.AI))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[me];
            c.AP += -playerCost;
            IAI ai = game.ai;
            if (ai != null && c.AP > 0)
            {
                while (c.AP > 0)
                {
                    float newCost = 1.0f;
                    ai.turn(me, nearCreatures, game, out newCost);
                    c.AP -= newCost;
                }
            }
            finishTurn(c);
        }

    }

    public void finishTurn(Creature c)
    {

    }

    public void finishPlayerTurn(IStack stack)
    {

        if (ENTITY.has(game.playerId, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[game.playerId];
            StainSystem.update(-c.AP, game);
            c.AP = 0f;
        }

        over(stack);
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
        bool over = false; //!game.player.alive();
        if (ENTITY.has(game.playerId, COMPONENT.DEFENSES))
        {
            Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[game.playerId];
            if (d.hp <= 0)
            {
                over = true;
            }
        }
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
