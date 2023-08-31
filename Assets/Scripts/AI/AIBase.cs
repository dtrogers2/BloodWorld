using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AIBase : IAI
{
    public bool turn(uint me, uint tgt, IGame game, out float actionCost)
    {
        ICmd cmd;
        Vector3Int dir = Vector3Int.zero;
        Position p1 = (Position) ComponentManager.get(COMPONENT.POSITION).data[me];
        Position p2 = (Position) ComponentManager.get(COMPONENT.POSITION).data[tgt];
        Vector3Int mePos = new Vector3Int(p1.x, p1.y, p1.z);
        
        Vector3Int tgtPos = new Vector3Int(p2.x, p2.y, p2.z);
        float tgtDistance = Vector3Int.Distance(mePos, tgtPos);
        if (tgtDistance <= 10)
        {
            if (tgtDistance < 2)
            {
                dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z);

                if (ENTITY.has(me, COMPONENT.PATH)) ENTITY.unsubscribe(me, COMPONENT.PATH);
                // Move directly to target
            }
            else if (tgtDistance >= 2)
            {
                if (Visbility.lineTo(tgtPos, mePos, game, false))
                {
                    // if it has unobstructed line, move directly towards target
                    dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z);
                    if (ENTITY.has(me, COMPONENT.PATH)) ENTITY.unsubscribe(me, COMPONENT.PATH);
                }
                else
                {
                    //dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z);
                    // Obstructed line towards target, find a path to the target
                    if (!ENTITY.has(me, COMPONENT.PATH))
                    {
                        Stack<Vector3Int> path = new Stack<Vector3Int>();
                        if (pathNext(game, mePos, tgtPos, out path))
                        {
                            ENTITY.subscribe(me, new Path { path = path , gScore = path.Count - 1, pathTurns = 0}, COMPONENT.PATH);
                        }
                    } 

                    if (ENTITY.has(me, COMPONENT.PATH))
                    {
                        Path p = (Path) ComponentManager.get(COMPONENT.PATH).data[me];
                        if (p.path.Count > 0)
                        {
                            dir = p.path.Pop();
                            Debug.Log($"hasPath dir: {dir}");
                        }

                        if (p.path.Count == 0) ENTITY.unsubscribe(me, COMPONENT.PATH);
                    }
                }
            }

            if (dir != Vector3Int.zero)
            {
                cmd = new BumpCmd(new Vector3Int(dir.x, dir.y, 0), me, game);
            } else
            {
                cmd = new WaitCmd(me, game);
            }
            
        } else
        {
            cmd = new WaitCmd(me, game);
        }

        if (ENTITY.has(me, COMPONENT.PATH))
        {
            Path p = (Path)ComponentManager.get(COMPONENT.PATH).data[me];
            p.pathTurns++;
            if (p.pathTurns >= p.gScore || p.gScore > Vector3Int.Distance(mePos, tgtPos))
            {
                ENTITY.unsubscribe(me, COMPONENT.PATH);
            }
        }
        return cmd.turn(out actionCost);
    }

    // Node positions are relative to the start position
    public bool pathNext(IGame game, Vector3Int pos, Vector3Int tgt, out Stack<Vector3Int> foundPath)
    {
        int searchMax = 100;
        foundPath = new Stack<Vector3Int>();
        Node[,] nodes = Node.getNodes(game, pos);
        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();
        // Vector3Int oCenter = new Vector3Int(Mathf.Abs(pos.x - Node.dim.x / 2), Mathf.Abs(pos.y - Node.dim.y / 2), pos.z);
        Node start = nodes[Node.dim.x / 2, Node.dim.y / 2];
        Node oDest = nodes[Node.dim.x / 2 + (tgt - pos).x, Node.dim.y / 2 + (tgt - pos).y];
        if (!start.traversable() || !oDest.traversable()) { Debug.Log("start or end not traversible"); return false; }
        start.resetNode();
        oDest.resetNode();
        Node current = start;
        Node goal = oDest;
        Node initGoal = goal;
        

        current.gScore = 0;
        current.hScore = start.distanceBetween(goal);
        open.Add(start);
        Node same = null;
        // If the starting position is the closest to the goal without having to move, don't move;
        if (current == goal) {Debug.Log($"Already at goal"); return false; }
        // The open set ocntains unevaluated paths to the goal
        while (open.Count > 0)
        {
            if (goal.blocks()) goal = Node.getUnblockedGoal(nodes, initGoal, start);
            searchMax--;
            if (searchMax == 0 || goal == null)
            {
                //Debug.Log($"Searched too many spaces ({searchMax}) or goal is {goal}. Open count: {open.Count}");
                return false;
            }
            //If the goal position is blocked, it creates a new goal position closest to the original goal position
           
            if (current != goal)
            {
                //This selects the node position in the open set closest to the goal that requires the fewest moves to get there
                foreach (Node node in open)
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
                //Vector3Int[] path = new Vector3Int[current.gScore + 1];
                int counter = current.gScore;
                while (counter > 0)
                {
                    //path[counter - 1] = current.position;
                    foundPath.Push(new Vector3Int(Sign(current.position.x), Sign(current.position.y)));
                    current = current.nodeParent;
                    counter--;
                }
                return true;
            }

            open.Remove(current);
            closed.Add(current);

            //Gets the node positions that are next to the current node
            List<Node> neighbors = current.getNeighbors(nodes);

            bool changeGoal = false;
            foreach (Node neighbor in neighbors)
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
        return false;

    }

    public static int Sign(int n)
    {
        return n < 0 ? -1 : (n > 0 ? 1 : 0);
    }
}
