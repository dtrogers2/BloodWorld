using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AIBase : IAI
{
    public bool turn(Creature me, Creature tgt, IGame game, out float actionCost)
    {
        ICmd cmd;
        Vector3Int dir = Vector3Int.zero;
        float tgtDistance = Vector3Int.Distance(me.position, tgt.position);
        if (tgtDistance <= 10)
        {
            if (tgtDistance < 2)
            {
                dir = new Vector3Int(Math.Sign(0f + tgt.position.x - me.position.x), Math.Sign(tgt.position.y - me.position.y), me.position.z);
                // Move directly to target
            }
            else if (tgtDistance >= 2)
            {
                if (Visbility.canSee(tgt.position, me.position, game, false))
                {
                    // if it has unobstructed line, move directly towards target
                    dir = new Vector3Int(Math.Sign(0f + tgt.position.x - me.position.x), Math.Sign(tgt.position.y - me.position.y), me.position.z);

                }
                else
                {
                    //dir = new Vector3Int(Math.Sign(0f + tgt.position.x - me.position.x), Math.Sign(tgt.position.y - me.position.y), me.position.z);
                    // Obstructed line towards target, find a path to the target
                    pathNext(game, me.position, tgt.position, out dir);
                }
            }
            cmd = new BumpCmd(new Vector3Int(dir.x, dir.y, 0), me, game);
        } else
        {
            cmd = new WaitCmd(me, game);
        }
       
        return cmd.turn(out actionCost);
    }

    public bool pathNext(IGame game, Vector3Int pos, Vector3Int tgt, out Vector3Int foundPath)
    {
        int searchMax = 100;
        foundPath = Vector3Int.zero;
        World world = game.world;
        List<Tile> open = new List<Tile>();
        List<Tile> closed = new List<Tile>();
        Tile start;
        Tile oDest;
        if (world.getTile(pos, game, out start) && world.getTile(tgt, game, out oDest))
        {
            if (!start.traversable() || !oDest.traversable()) { return false; }
        }  else
        {
            return false;
        }
        start.resetNode();
        oDest.resetNode();
        Tile current = start;
        Tile goal = oDest;
        Tile initGoal = goal;
        

        current.gScore = 0;
        current.hScore = start.distanceBetween(goal);
        open.Add(start);
        Tile same = null;
        // If the starting position is the closest to the goal without having to move, don't move;
        if (current == goal) {current.resetNode();goal.resetNode(); return false; }
        // The open set ocntains unevaluated paths to the goal
        while (open.Count > 0)
        {
            if (goal.blocks()) goal = world.getUnblockedGoal(game, initGoal, start);
            searchMax--;
            if (searchMax == 0 || goal == null)
            {
                foreach (Tile t in closed)
                {
                    t.resetNode();
                }

                foreach (Tile t in open)
                {
                    t.resetNode();
                }
                current.resetNode();
                return false;
            }
            //If the goal position is blocked, it creates a new goal position closest to the original goal position
           
            if (current != goal)
            {
                //This selects the node position in the open set closest to the goal that requires the fewest moves to get there
                foreach (Tile node in open)
                {
                    if ((node.gScore + node.hScore < current.gScore + current.hScore))
                    {
                        current = node;
                    }
                }
                //if there are multiple nodes that are roughly equal, it chooses the most recently added on to evaluate
                if (current == same)
                {
                    current = open[open.Count - 1];
                }
            }
            //If a path is found, it returns the list of moves it takes to get to the goal position
            if (current == goal)
            {
                Vector3Int[] path = new Vector3Int[current.gScore + 1];
                int counter = current.gScore;
                while (counter > 0)
                {
                    path[counter - 1] = current.position;
                    current = current.nodeParent;
                    counter--;
                }
                foundPath = path[0] - start.position;
                foreach (Tile t in closed)
                {
                    t.resetNode();
                }

                foreach (Tile t in open)
                {
                    t.resetNode();
                }
                return true;
            }

            open.Remove(current);
            closed.Add(current);

            //Gets the node positions that are next to the current node
            List<Tile> neighbors = world.getNeighbors(game, current.position);

            bool changeGoal = false;
            foreach (Tile neighbor in neighbors)
            {
                // Possibly redundant, neighbors no longer contains blocked tiles
                /* if (neighbor.blocks())
                {
                    closed.Add(neighbor);
                    continue;
                }*/
                //keeps the old g score temporarily
                //checks if the updated gscore is better than the old one
                if (neighbor.visited)
                {
                    int oldG = neighbor.getG();
                    neighbor.setG(current);
                    if (oldG + neighbor.hScore > neighbor.gScore + neighbor.hScore)
                    {
                        neighbor.nodeParent = current;
                    }
                    else
                    { neighbor.gScore = oldG; }

                }
                //If the neighbor hasn't been visited yet and it's not blocked, then the node is added to the open set
                if (!closed.Contains(neighbor) ) //&& !neighbor.blocks())//(!neighbor.blocked)
                {

                    open.Add(neighbor);
                    neighbor.setG(current);
                    neighbor.setH(goal);
                    neighbor.nodeParent = current;
                }
                neighbor.visited = true;
            }
            if (!changeGoal) same = current;
        }
        // Cleanup tiles nodes
        foreach (Tile t in closed)
        {
            Debug.Log(t.position);
            t.resetNode();
        }
        return false;

    }
}
