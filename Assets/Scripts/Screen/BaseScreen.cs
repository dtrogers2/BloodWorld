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
        nearCreatures.Add(game.playerId);
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
            finishTurn(me, -playerCost);
        }

    }

    public void finishTurn(uint id, float time)
    {
        if (ENTITY.has(id, COMPONENT.DEFENSES))
        {
            // Regen HP
            Defenses d = (Defenses)ComponentManager.get(COMPONENT.DEFENSES).data[id];
            d.regenAmt += time * d.regenRate;
            if (d.regenAmt >= 1)
            {
                int amt = Mathf.FloorToInt(d.regenAmt);
                HealthAdj.heal(id, amt);
                d.regenAmt -= amt;
            }
        }
    }

    public void finishPlayerTurn(IStack stack)
    {

        itemsHere();

        if (ENTITY.has(game.playerId, COMPONENT.CREATURE))
        {
            Creature c = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[game.playerId];
            finishTurn(game.playerId, -c.AP);
            StainSystem.update(-c.AP, game);
            c.AP = 0f;
        }

        over(stack);
    }

    public void itemsHere()
    {
        if (ENTITY.has(game.playerId, COMPONENT.POSITION) && ENTITY.has(game.playerId, COMPONENT.CELLSTACK))
        {
            Msg itemMsg = new Msg();
            itemMsg.color = COLOR.White;
            itemMsg.text = $"You see here: a ";
            CellStack s = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[game.playerId];
            uint entity = s.entity;
            bool hasItem = false;
            while (ENTITY.has(entity, COMPONENT.ITEM))
            {
                Item i = (Item)ComponentManager.get(COMPONENT.ITEM).data[entity];
                itemMsg.text += $"{(hasItem ? ",": "")}{i.name}";
                hasItem = true;
                if (ENTITY.has(entity, COMPONENT.CELLSTACK)) {
                    s = (CellStack)ComponentManager.get(COMPONENT.CELLSTACK).data[entity];
                    entity = s.entity;
                } else
                {
                    break;
                }
            }

            if (hasItem) game.msg(itemMsg);
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
