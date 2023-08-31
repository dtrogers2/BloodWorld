using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Node : IComparer
{
    public Vector3Int position { get; set; }
    public int gScore { set; get; }
    public int hScore { set; get; }
    public bool visited { set; get; }
    public Node nodeParent { set; get; }
    public uint cellFlags { get; set; }

    public static Vector2Int dim { get; } = new Vector2Int(30, 30);

    public static Node[,] getNodes(IGame game, Vector3Int entityPosition)
    {
        Node[,] nodes = new Node[dim.x,dim.y];
        for (int y = 0; y < dim.y; y++)
        {
            
            for (int x = 0; x < dim.x; x++)
            {
                if (game.world.getCellFlags(new Vector3Int(entityPosition.x - (dim.x / 2) + x, entityPosition.y - (dim.y / 2) + y), game, out uint cellFlags))
                {
                    nodes[x, y] = new Node(new Vector3Int(x - (dim.x / 2), y - (dim.y / 2), entityPosition.z), cellFlags);
                } else
                {
                    nodes[x, y] = new Node(new Vector3Int(x - (dim.x / 2), y - (dim.y / 2), entityPosition.z), (uint) CELLFLAG.BLOCKED);
                }
            }
        }
        return nodes;
    }

    public int getF()
    {
        return gScore + hScore;
    }

    public Node(Vector3Int position)
    {
        cellFlags = 0;
        gScore = int.MaxValue;
        hScore = int.MaxValue;
        this.position = position;
    }

    public Node(Vector3Int position, uint flags)
    {
        gScore = int.MaxValue;
        hScore = int.MaxValue;
        this.position = position;
        cellFlags = flags;
    }

    public bool blocks()
    {
        return ENTITY.bitHas(cellFlags, (uint) ( CELLFLAG.BLOCKED | CELLFLAG.CREATURE));
    }

    public bool traversable()
    {
        return !ENTITY.bitHas(cellFlags, (uint)CELLFLAG.BLOCKED);
    }
    public void setG(Node previous)
    {
        gScore = previous.getG() + 1;
    }

    public int getG()
    {
        return gScore;
    }

    public void setH(Node goal)
    {
        hScore = distanceBetween(goal);
    }

    public int getH()
    {
        return hScore;
    }

    public int distanceBetween(Node goal)
    {
        return Mathf.Abs(goal.position.x - position.x) + Mathf.Abs(goal.position.y - position.y) + Mathf.Abs(goal.position.z - position.z);
    }
    int IComparer.Compare(object a, object b)
    {
        Node center = (Node)b;
        Node n2 = (Node)a;
        double dist1 = distanceBetween(center);
        double dist2 = n2.distanceBetween(center);
        if (dist1 > dist2) return 1;
        if (dist1 < dist2) return -1;
        else return 0;
    }

    public static bool getNode(Node[,]nodes, Vector3Int position, out Node n)
    {
        n = null;
        if (position.x < -(dim.x / 2) || position.x >= dim.x / 2|| position.y < -(dim.x / 2)
            || position.y >= dim.y / 2) return false;
        n = nodes[position.x + (dim.x / 2), position.y + (dim.x / 2)];
        return true;
    }

    public static Node getUnblockedGoal(Node[,] nodes, Node start, Node originalGoal)
    {
        int loopMax = 3;
        bool found = false;
        int xmin = 0;
        int xmax = 0;
        int ymin = 0;
        int ymax = 0;
        Node newGoal = start;

        do
        {
            for (int y = ymin; y <= ymax; y++)
            {
                for (int x = xmin; x <= xmax; x++)
                {
                    if (getNode(nodes, new Vector3Int(start.position.x + x, start.position.y + y), out Node t))
                    {
                        if (!t.blocks())
                        {

                            if (!found)
                            {
                                newGoal = t;
                                found = true;
                            }
                            if (found == true)
                            {
                                if ((t.distanceBetween(start) <= newGoal.distanceBetween(start)) && t.distanceBetween(originalGoal) <= newGoal.distanceBetween(originalGoal))
                                {
                                    newGoal = t;
                                }
                            }
                        }
                    }
                    else
                    {
                        continue;
                    }

                }
            }
            if (xmin - 1 > 0) xmin--;
            if (xmax + 1 < dim.x) xmax++;
            if (ymin - 1 > 0) ymin--;
            if (ymax + 1 < dim.y) ymax++;
            loopMax--;
            if (loopMax == 0) return null;
        } while (found == false);
        return newGoal;
    }


    public List<Node> getNeighbors(Node[,] nodes)
    {
        List<Node> retList = new List<Node>();
        for (int y = -1; y <= 1; y++)
        {
            for (int x = -1; x <= 1; x++)
            {
                if (y == 0 && x == 0) continue;
                if (getNode(nodes, new Vector3Int(position.x + x, position.y + y), out Node n))
                {
                    if (n.traversable()) retList.Add(n);
                }
            }
        }
        return retList;
    }
}
