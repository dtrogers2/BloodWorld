using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : IAI
{
    public bool turn(Creature me, Creature tgt, IGame game, out float actionCost)
    {
        Vector2Int dir = Vector2Int.zero;
        if (pathNext(game.curMap(), me.position, tgt.position, out Vector2Int path))
        {
            
        };
        //Vector2Int dir = game.rng.rndDir0();
        ICmd cmd = new MoveCmd(new Vector3Int(path.x, path.y, 0), me, game);
        return cmd.turn(out actionCost);
    }

    public bool pathNext(IRegion map, Vector3Int pos, Vector3Int tgt, out Vector2Int foundPath)
    {
        foundPath = Vector2Int.zero;
        Region nodes = (Region)map;
        nodes.resetNodes();
        List<Tile> open = new List<Tile>();
        List<Tile> closed = new List<Tile>();
        if (!nodes.legal((Vector2Int) pos) || !nodes.legal((Vector2Int) tgt)) { return false; }
        Tile start = nodes.tile((Vector2Int) pos);
        Tile current = start;
        Tile goal = nodes.tile((Vector2Int) tgt);
        Tile initGoal = goal;
        

        current.gScore = 0;
        current.hScore = start.distanceBetween(goal);
        open.Add(start);
        Tile same = null;
        // If the starting position is the closest to the goal without having to move, don't move;
        if (current == goal) { return false; }
        // The open set ocntains unevaluated paths to the goal
        while (open.Count > 0)
        {
            //If the goal position is blocked, it creates a new goal position closest to the original goal position
            if (nodes.blocked(goal.position)) goal = nodes.getUnblockedGoal(initGoal, start);
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
                Vector2Int[] path = new Vector2Int[current.gScore + 1];
                int counter = current.gScore;
                while (counter > 0)
                {
                    path[counter - 1] = current.position;
                    current = current.nodeParent;
                    counter--;
                }
                foundPath = path[0] - start.position;
                return true;
            }

            open.Remove(current);
            closed.Add(current);

            //Gets the node positions that are next to the current node
            List<Tile> neighbors = nodes.getNeighbors(current.position);

            bool changeGoal = false;
            foreach (Tile neighbor in neighbors)
            {
                if (nodes.blocked(neighbor.position))
                {
                    closed.Add(neighbor);
                    continue;
                }
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
                if (!closed.Contains(neighbor) && !nodes.blocked(neighbor.position))//(!neighbor.blocked)
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
        return false;

    }
}
