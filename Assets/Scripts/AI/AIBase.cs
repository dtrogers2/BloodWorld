using System;
using System.Collections.Generic;
using UnityEngine;

public class AIBase : IAI
{
    public bool turn(uint me, List<uint> nearCreatures, IGame game, out float actionCost)
    {

        ICmd cmd;
        
        AI ai = (AI)ComponentManager.get(COMPONENT.AI).data[me];
        // Choose state based on what is happening
        ai.state = handleState(game, me, ai, nearCreatures);
        // switch based on state
        switch (ai.state)
        {
            case STATE.CHASE: cmd = handleChase(game, me, ai); break;
            case STATE.IDLE: cmd = handleIdle(game, me); break;
            case STATE.REST: cmd = handleRest(game, me); break;
            case STATE.INVESTIGATE: cmd = handleInvestigate(game, me, ai); break;
            case STATE.WANDER: cmd = handleWander(game, me, ai); break;
            case STATE.FOLLOW: cmd = handleFollow(game, me, ai); break;
            default: cmd = handleIdle(game, me); break;
        }
        return cmd.turn(out actionCost);
    }

    public STATE handleState(IGame game, uint me, AI ai, List<uint> nearCreatures)
    {

        // Clear targets and leaders
        if (!ENTITY.has(ai.target, COMPONENT.CREATURE)) ai.target = 0;
        if (!ENTITY.has(ai.leader, COMPONENT.CREATURE)) ai.leader = 0;
        // if I have a target, chase that target
        if (ai.target != 0 && ai.memory > 0 && ai.aggro > 0)
        {
             return STATE.CHASE;
        }
        // If I have a leader
        if (ai.leader != 0)
        {
            // See if the leader has a target, if so then target the leaders target
            if (ENTITY.has(ai.leader, COMPONENT.AI))
            {
                AI aiL = (AI)ComponentManager.get(COMPONENT.AI).data[ai.leader];
                if (aiL.target != 0 && ENTITY.has(aiL.target, COMPONENT.CREATURE))
                {
                    ai.target = aiL.target;
                    ai.memory = 1;
                    return STATE.CHASE;
                }
            }
            // Otherwise follow the leader
            return STATE.FOLLOW;
        }

        // if there is no target and no leader, see if there is a hostile visible
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[me];
        Creature c1 = (Creature)ComponentManager.get(COMPONENT.CREATURE).data[me];
        if (ENTITY.has(me, COMPONENT.EGO))
        {
            Ego e1 = (Ego)ComponentManager.get(COMPONENT.EGO).data[me];
            uint nearestHostile = 0;
            float nearestDistance = float.MaxValue;
            for (int i = 0; i < nearCreatures.Count; i++)
            {
                if (me == nearCreatures[i]) continue;
                if (!ENTITY.has(nearCreatures[i], COMPONENT.EGO)) continue;
                Position p2 = (Position)ComponentManager.get(COMPONENT.POSITION).data[nearCreatures[i]];
                Vector3Int mePos = new Vector3Int(p1.x, p1.y, p1.z);
                Vector3Int tgtPos = new Vector3Int(p2.x, p2.y, p2.z);
                float distance = Vector3Int.Distance(mePos, tgtPos);
                int vision = 8;
                if (ENTITY.has(me, COMPONENT.CREATURE)) { vision = c1.vision; }
                if (distance > vision || !Visbility.lineTo(mePos, tgtPos, game, true) ) continue;
                int moodAdj = (e1.mood == MOOD.FRIENDLY) ? 250 : (e1.mood == MOOD.DOCILE) ? 100 : (e1.mood == MOOD.NEUTRAL) ? 0 : -250;
                Ego e2 = (Ego)ComponentManager.get(COMPONENT.EGO).data[nearCreatures[i]];
                for (int u = 0; u < Enum.GetNames(typeof(FAC)).Length; u++)
                {
                    if (e2.factions.HasFlag((FAC)(1 << u)))
                    {
                        int index = Array.IndexOf(Enum.GetValues(e2.factions.GetType()), (FAC)(1 << u));
                        if (e1.reputations[index] + moodAdj < -250 && distance < nearestDistance)
                        {
                            nearestHostile = nearCreatures[i];
                            nearestDistance = distance;
                        }
                    }
                }

            }

            if (nearestHostile != 0)
            {
                ai.target = nearestHostile;
                ai.memory = ai.memoryMax;
                ai.aggro = 0;
                return STATE.CHASE;
            }
        }


        // There is no current target, no leader, and no visible target.
        //TODO investigate noises
        // If already wandering, check to see if it has arrived at its destination
        if (ai.state == STATE.WANDER)
        {
            Position p2 = ai.pos;
            Vector3Int mePos = new Vector3Int(p1.x, p1.y, p1.z);
            Vector3Int tgtPos = new Vector3Int(p2.x, p2.y, p2.z);
            float tgtDistance = Vector3Int.Distance(mePos, tgtPos);
            if (tgtDistance < 2)
            {
                return STATE.IDLE;
            } else
            {
                return STATE.WANDER;
            }
        }
        // Roll to see if it is going to wander around
        
        
        if (game.rng.pct(16) && ai.state != STATE.WANDER)
        {

            int x = p1.x - 14 + game.rng.rng(28);
            int y = p1.y - 14 + game.rng.rng(28);
            if (game.world.getCellFlags(new Vector3Int(x, y), game, out uint cellFlag)) {
                if (!ENTITY.bitHas(cellFlag, (uint) CELLFLAG.BLOCKED))
                {
                    
                    ai.pos.x = x;
                    ai.pos.y = y;
                    ai.memory = ai.memoryMax;
                    return STATE.WANDER;
                }
            }
        }
        // Nothing to do, become idle
        return STATE.IDLE;
    }

    public ICmd handleIdle(IGame game, uint me)
    {
        return new WaitCmd(me, game);
    }

    public ICmd handleRest(IGame game, uint me)
    {
        return new WaitCmd(me, game);
    }

    public ICmd handleWander(IGame game, uint me, AI ai)
    {
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[me];
        Position p2 = ai.pos;
        Vector3Int mePos = new Vector3Int(p1.x, p1.y, p1.z);
        Vector3Int tgtPos = new Vector3Int(p2.x, p2.y, p2.z);
        float tgtDistance = Vector3Int.Distance(mePos, tgtPos);
        Vector3Int dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z); ;

        if (Visbility.lineTo(mePos, tgtPos, game, false))
        {
                dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z);
        }
        else
        {
             Stack<Vector3Int> path = new Stack<Vector3Int>();
             if (pathNext(game, mePos, tgtPos, out path))
             {
                dir = path.Pop();
             } else
             {
                ai.state = STATE.IDLE;
             }
        }
        return new BumpCmd(new Vector3Int(dir.x, dir.y, 0), me, game);
    }

    public ICmd handleInvestigate(IGame game, uint me, AI ai)
    {
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[me];
        Position p2 = ai.pos;
        Vector3Int mePos = new Vector3Int(p1.x, p1.y, p1.z);
        Vector3Int tgtPos = new Vector3Int(p2.x, p2.y, p2.z);
        float tgtDistance = Vector3Int.Distance(mePos, tgtPos);
        Vector3Int dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z); ;

        // See if there is an visible line to target, if so path towards it
        if (!Visbility.lineTo(mePos, tgtPos, game, true) && ai.memory > 0)
        {
            Stack<Vector3Int> path = new Stack<Vector3Int>();
            if (pathNext(game, mePos, tgtPos, out path))
                {
                    dir = path.Pop();
                
                }
            ai.memory--;
            return new BumpCmd(new Vector3Int(dir.x, dir.y, 0), me, game);
        } 
        {
            // Either memory ran out, or it can see the position it was trying to investigate, so become idle
            ai.state = STATE.IDLE;
        }
        return new WaitCmd(me, game);
        
    }

    public ICmd handleFollow(IGame game, uint me, AI ai)
    {
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[me];
        Position p2 = (Position)ComponentManager.get(COMPONENT.POSITION).data[ai.leader];
        Vector3Int mePos = new Vector3Int(p1.x, p1.y, p1.z);
        Vector3Int tgtPos = new Vector3Int(p2.x, p2.y, p2.z);
        float tgtDistance = Vector3Int.Distance(mePos, tgtPos);
        Vector3Int dir = Vector3Int.zero;

        // See if there is an visible line to target, if so path towards it
        if (Visbility.lineTo(mePos, tgtPos, game, true) && tgtDistance >= 2)
        {
            if (Visbility.lineTo(mePos, tgtPos, game, false))
            {
                dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z);
            }
            else
            {
                Stack<Vector3Int> path = new Stack<Vector3Int>();
                if (pathNext(game, mePos, tgtPos, out path))
                {
                    dir = path.Pop();
                }
            }
        }
        else if (tgtDistance >= 2)// There is no visible line to the target
        {
            Stack<Vector3Int> path = new Stack<Vector3Int>();
            if (pathNext(game, mePos, tgtPos, out path))
            {
                dir = path.Pop();
            }
        }
        if (dir != Vector3Int.zero)
        {
            return new BumpCmd(new Vector3Int(dir.x, dir.y, 0), me, game);
        }
        else
        {
            return new WaitCmd(me, game);
        }
    }

    public ICmd handleChase(IGame game, uint me, AI ai)
    {
        Position p1 = (Position)ComponentManager.get(COMPONENT.POSITION).data[me];
        Position p2 = (Position)ComponentManager.get(COMPONENT.POSITION).data[ai.target];
        Vector3Int mePos = new Vector3Int(p1.x, p1.y, p1.z);
        Vector3Int tgtPos = new Vector3Int(p2.x, p2.y, p2.z);
        float tgtDistance = Vector3Int.Distance(mePos, tgtPos);
        Vector3Int dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z); ;

        // See if there is an visible line to target, if so path towards it
        if (Visbility.lineTo(mePos, tgtPos, game, true) && tgtDistance >= 2)
        {
            if (Visbility.lineTo(mePos, tgtPos, game, false))
            {
              dir = new Vector3Int(Math.Sign(0f + tgtPos.x - mePos.x), Math.Sign(tgtPos.y - mePos.y), mePos.z);
            } else if (tgtDistance < 15)
            {
                Stack<Vector3Int> path = new Stack<Vector3Int>();
                if (pathNext(game, mePos, tgtPos, out path))
                {
                    dir = path.Pop();
                }
            }
            ai.memory = ai.memoryMax;
            
        } else if (ai.memory > 0 && tgtDistance >= 2 && tgtDistance < 15) // There is no visible line to the target
        {
            Stack<Vector3Int> path = new Stack<Vector3Int>();
            if (pathNext(game, mePos, tgtPos, out path))
            {
                dir = path.Pop();
            }
            ai.memory--;
        }
        return new BumpCmd(new Vector3Int(dir.x, dir.y, 0), me, game);
    }

    // Node positions are relative to the start position
    public bool pathNext(IGame game, Vector3Int pos, Vector3Int tgt, out Stack<Vector3Int> foundPath)
    {
        //int searchMax = 100;
        foundPath = new Stack<Vector3Int>();
        Node[,] nodes = Node.getNodes(game, pos);
        List<Node> open = new List<Node>();
        HashSet<Node> closed = new HashSet<Node>();
        // Vector3Int oCenter = new Vector3Int(Mathf.Abs(pos.x - Node.dim.x / 2), Mathf.Abs(pos.y - Node.dim.y / 2), pos.z);
        Node start = nodes[Node.dim.x / 2, Node.dim.y / 2];
        if (Node.dim.x / 2 + (tgt - pos).x < 0 || Node.dim.x / 2 + (tgt - pos).x >= Node.dim.x || Node.dim.y / 2 + (tgt - pos).y < 0 || Node.dim.y / 2 + (tgt - pos).y >= Node.dim.y) return false;
        Node goal = nodes[Node.dim.x / 2 + (tgt - pos).x, Node.dim.y / 2 + (tgt - pos).y];
        // if (!start.traversable() || !oDest.traversable()) { Debug.Log("start or end not traversible"); return false; }
        open.Add(start);

        while (open.Count > 0)
        {
            Node current = open[0];
            for (int i = 0; i < open.Count; i++)
            {
                if (open[i].getF() < current.getF() || open[i].getF() == current.getF()) 
                {
                    if (open[i].hScore < current.hScore)
                    current = open[i];
                }

            }
            open.Remove(current);
            closed.Add(current);

            if (current == goal)
            {
                while (current.nodeParent != null)
                {
                    foundPath.Push(current.position);
                    current = current.nodeParent;
                }
                return true;
            }

            foreach (Node neighbor in current.getNeighbors(nodes))
            {
                if (closed.Contains(neighbor) || !neighbor.traversable()) continue;
                int newCost = current.gScore + current.distanceBetween(neighbor);
                if (!open.Contains(neighbor) || newCost < neighbor.gScore)
                {
                    neighbor.gScore = newCost;
                    neighbor.hScore = neighbor.distanceBetween(goal);
                    neighbor.nodeParent = current;
                    if (!open.Contains(neighbor)) open.Add(neighbor);
                }
            }
        }
        //Node same = null;
        /*
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
                    Vector3Int s = new Vector3Int(Sign(current.position.x), Sign(current.position.y));
                    if (s != Vector3Int.zero) foundPath.Push(s);
                    Debug.Log($"path[{counter}]:{current.position}");
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
        }*/
        return false;

    }

    public static int Sign(int n)
    {
        return n < 0 ? -1 : (n > 0 ? 1 : 0);
    }
}
