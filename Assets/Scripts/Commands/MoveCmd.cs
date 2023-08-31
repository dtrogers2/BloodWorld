using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCmd : CmdBase
{
    Vector3Int dir;
    public MoveCmd(Vector3Int dir, uint me, IGame game) : base(me, game)
    {
        this.dir = dir;
    }

    public override bool exc()
    {
        Position p = (Position) ComponentManager.get(COMPONENT.POSITION).data[me];
        Vector3Int oldPos = new Vector3Int(p.x, p.y, p.z);
        Vector3Int newPos = new Vector3Int(p.x + dir.x, p.y + dir.y, p.z + dir.z);
        bool legal = game.world.moveEntity(me, newPos, game);
        if (legal)
        {
            // Leave stain at old position
            if (ENTITY.has(me, COMPONENT.STAIN))
            {
                Stain s = (Stain)ComponentManager.get(COMPONENT.STAIN).data[me];
                s.amount--;
                Stack<uint> sStack = new Stack<uint>();
                if (game.world.getCellStack(oldPos, game, out sStack))// if there is a cellstack, iterate through the stack to get the bottom and stain it
                {
                    
                    uint cellEntity = sStack.Pop();
                    if (!Env.isEnv(cellEntity))
                    {
                        if (ENTITY.has(cellEntity, COMPONENT.STAIN))
                        {
                            Stain otherS = (Stain)ComponentManager.get(COMPONENT.STAIN).data[cellEntity];
                            otherS.stainflags = ENTITY.bitSet(otherS.stainflags, s.stainflags);
                            otherS.amount += s.amount;
                        } else
                        {
                            ENTITY.subscribe(cellEntity, new Stain { stainflags = s.stainflags, amount = 5, isPool = false }, COMPONENT.STAIN);
                        }
                    }
                } else if (game.world.getCellEntity(oldPos, game, out uint cellEnt))// If there is no cellstack, add it straight to the cell
                {
                    // If not already a pool at the env, can add stain to the entity
                    if (!(Env.isEnv(cellEnt) && ENTITY.has(cellEnt, COMPONENT.STAIN))) {
                        uint newId = EntityManager.create();
                        ENTITY.subscribe(newId, new object[3] { new Glyph { c = '~', color = ColorHex.Blue }, new Position { x = oldPos.x, y = oldPos.y, z = oldPos.z }, new Stain { stainflags = s.stainflags, amount = 5, isPool = false } }, new COMPONENT[3] { COMPONENT.GLYPH, COMPONENT.POSITION, COMPONENT.STAIN });
                        game.world.addEntity(newId, game);
                    }
                }

                if (s.amount <= 0) ENTITY.unsubscribe(me, COMPONENT.STAIN);
            }
            //Get Stain at new position
            Stack<uint> stack = new Stack<uint>();
            if (game.world.getCellStack(newPos, game, out stack))
            {
                uint id = stack.Pop();
                if (ENTITY.has(id, COMPONENT.STAIN))
                {
                    Stain s = (Stain)ComponentManager.get(COMPONENT.STAIN).data[id];
                    if (s.isPool)
                    {
                        if (ENTITY.has(me, COMPONENT.STAIN) && s.isPool)
                        {
                            Stain mS = (Stain)ComponentManager.get(COMPONENT.STAIN).data[me];
                            ENTITY.bitSet(mS.stainflags, (uint)s.stainflags);
                            mS.amount = 10;
                        } else
                        {
                            ENTITY.subscribe(me, new object[1] { new Stain { stainflags = s.stainflags, isPool = false, amount = 10 } }, COMPONENT.STAIN);
                        }
                    }
                }
            }
        }
        return legal;
    }

    public override bool turn(out float actionCost)
    {
        actionCost = 1.0f;
        if (ENTITY.has(me, COMPONENT.CREATURE))
        {
            Creature meC = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[me];
            actionCost = meC.moveRate;

        }

        return this.exc();
    }


}
