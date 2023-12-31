using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class StainSystem
{
    public static void leaveStains(uint entity, IGame game, Vector3Int position)
    {
        Stain s = (Stain)ComponentManager.get(COMPONENT.STAIN).data[entity];
        Stack<uint> sStack = new Stack<uint>();
        if (game.world.getCellStack(position, game, out sStack))// if there is a cellstack, iterate through the stack to get the bottom and stain it
        {

            uint cellEntity = sStack.Pop();
            if (!Env.isEnv(cellEntity))
            {
                if (ENTITY.has(cellEntity, COMPONENT.STAIN))
                {
                    Stain otherS = (Stain)ComponentManager.get(COMPONENT.STAIN).data[cellEntity];
                    otherS.stainflags = ENTITY.bitSet(otherS.stainflags, s.stainflags);
                    otherS.amount += s.amount;
                }
                else
                {
                    ENTITY.subscribe(cellEntity, new Stain { stainflags = s.stainflags, amount = 5, isPool = false });
                }
            }
        }
        else if (game.world.getCellEntity(position, game, out uint cellEnt))// If there is no cellstack, add it straight to the cell
        {
            // If not already a pool at the env, can add stain to the entity
            if (Env.isEnv(cellEnt) && !ENTITY.has(cellEnt, COMPONENT.STAIN))
            {
                char eC = '.';
                if (ENTITY.has(cellEnt, COMPONENT.GLYPH))
                {
                    Glyph g = (Glyph)ComponentManager.get(COMPONENT.GLYPH).data[cellEnt];
                    eC = g.c;
                }
                uint newId = EntityManager.create();
                ENTITY.subscribe(newId, new object[3] { new Glyph { c = eC, color = COLOR.Blue }, new Position { x = position.x, y = position.y, z = position.z }, new Stain { stainflags = s.stainflags, amount = 5, isPool = false } });
                game.world.addEntity(newId, game);
            }
            else
            if (!(Env.isEnv(cellEnt) && ENTITY.has(cellEnt, COMPONENT.STAIN)))
            {
                uint newId = EntityManager.create();
                ENTITY.subscribe(newId, new object[3] { new Glyph { c = '~', color = COLOR.Blue }, new Position { x = position.x, y = position.y, z = position.z }, new Stain { stainflags = s.stainflags, amount = 5, isPool = false } });
                game.world.addEntity(newId, game);
            }
        }
        s.amount--;
        if (s.amount <= 0) ENTITY.unsubscribe(entity, COMPONENT.STAIN);
    }

    public static void gainStains(uint entity, IGame game, Vector3Int position)
    {
        Stack<uint> stack = new Stack<uint>();
        if (game.world.getCellStack(position, game, out stack))
        {
            uint id = stack.Pop();
            if (ENTITY.has(id, COMPONENT.STAIN))
            {
                Stain s = (Stain)ComponentManager.get(COMPONENT.STAIN).data[id];
                if (s.isPool)
                {
                    if (ENTITY.has(entity, COMPONENT.STAIN) && s.isPool)
                    {
                        Stain mS = (Stain)ComponentManager.get(COMPONENT.STAIN).data[entity];
                        ENTITY.bitSet(mS.stainflags, (uint)s.stainflags);
                        mS.amount = 10;
                    }
                    else
                    {
                        ENTITY.subscribe(entity, new object[1] { new Stain { stainflags = s.stainflags, isPool = false, amount = 10 } });
                    }
                }
            }
        }
    }

    public static void update(float time, IGame game)
    {
        for (int stainIndex = ComponentManager.get(COMPONENT.STAIN).entities.Count - 1; stainIndex >= 0; stainIndex--)
        {

            uint stainId = ComponentManager.get(COMPONENT.STAIN).entities[stainIndex];
            if (!Env.isEnv(stainId))
            {

                Stain s = (Stain)ComponentManager.get(COMPONENT.STAIN).data[stainId];
                Position p = (Position)ComponentManager.get(COMPONENT.POSITION).data[stainId];
                s.amount -= time;

                if (s.amount <= 0)
                {
                    ENTITY.unsubscribe(stainId, COMPONENT.STAIN);
                    uint flags = ENTITY.get(stainId);

                    if (ENTITY.bitIs(flags, (uint)(COMPONENT.GLYPH | COMPONENT.POSITION | COMPONENT.CELLSTACK))) // If stain was just a stain entity, recycle it
                    {
                        game.world.removeEntity(stainId, game);
                        ENTITY.unsubscribe(stainId, new COMPONENT[2] { COMPONENT.GLYPH, COMPONENT.POSITION });
                    }
                }
            }
        }
    }
}
