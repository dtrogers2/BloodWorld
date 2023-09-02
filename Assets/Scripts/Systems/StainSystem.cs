using System.Collections;
using System.Collections.Generic;
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
                    ENTITY.subscribe(cellEntity, new Stain { stainflags = s.stainflags, amount = 5, isPool = false }, COMPONENT.STAIN);
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
                ENTITY.subscribe(newId, new object[3] { new Glyph { c = eC, color = ColorHex.Blue }, new Position { x = position.x, y = position.y, z = position.z }, new Stain { stainflags = s.stainflags, amount = 5, isPool = false } }, new COMPONENT[3] { COMPONENT.GLYPH, COMPONENT.POSITION, COMPONENT.STAIN });
                game.world.addEntity(newId, game);
            }
            else
            if (!(Env.isEnv(cellEnt) && ENTITY.has(cellEnt, COMPONENT.STAIN)))
            {
                uint newId = EntityManager.create();
                ENTITY.subscribe(newId, new object[3] { new Glyph { c = '~', color = ColorHex.Blue }, new Position { x = position.x, y = position.y, z = position.z }, new Stain { stainflags = s.stainflags, amount = 5, isPool = false } }, new COMPONENT[3] { COMPONENT.GLYPH, COMPONENT.POSITION, COMPONENT.STAIN });
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
                        ENTITY.subscribe(entity, new object[1] { new Stain { stainflags = s.stainflags, isPool = false, amount = 10 } }, COMPONENT.STAIN);
                    }
                }
            }
        }
    }

    public static void update(float time, IGame game)
    {
        for (int stainCount = ComponentManager.get(COMPONENT.STAIN).entities.Count - 1; stainCount > 0; stainCount--)
        {
            uint stainId = ComponentManager.get(COMPONENT.STAIN).entities[stainCount];
            if (!Env.isEnv(stainId))
            {

                Stain s = (Stain)ComponentManager.get(COMPONENT.STAIN).data[stainId];
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
