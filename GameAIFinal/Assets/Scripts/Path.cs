/* This game uses the A\* pathfinding algorithm, which is implemented based on pseudocode found in a YouTube video tutorial by Sebastian Lague.
 * The citation for the video can be found in the readme file at the root directory of this project.
 */

using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Path
{
    Square origin;
    Square target;
    public Square Target { get { return target; } }

    List<SquareNode> pathList = new List<SquareNode>();
    public List<SquareNode> PathList { get { return pathList; } }
    public int Length { get { return pathList.Count; } }

    public Path(Square origin, Square target)
    {
        this.origin = origin;
        this.target = target;

        makePath();
    }

    public void makePath()
    {
        List<SquareNode> open = new List<SquareNode>();
        List<SquareNode> closed = new List<SquareNode>();

        SquareNode current = new SquareNode(origin, target);
        closed.Add(current);

        while (current.Square != target)
        {
            SquareNode[] neighborNodes = current.getNeighborNodes();
            bool[] alreadyExists = new bool[neighborNodes.Length];

            for(int i = 0; i < neighborNodes.Length; i++)
            {
                if (neighborNodes[i] != null)
                {
                    foreach(SquareNode n in open)
                    {
                        if (neighborNodes[i].Equals(n))
                        {
                            neighborNodes[i] = n;
                            alreadyExists[i] = true;
                        }
                    }

                    foreach(SquareNode n in closed)
                    {
                        if (neighborNodes[i].Equals(n))
                        {
                            neighborNodes[i] = n;
                            alreadyExists[i] = true;
                        }
                    }
                }
            }

            for(int i = 0; i < neighborNodes.Length; i++)
            {
                if (neighborNodes[i] != null)
                {
                    if (SquareNode.getNewGCost(current, neighborNodes[i]) < neighborNodes[i].GCost)
                    {
                        neighborNodes[i].Previous = current;
                    }

                    if (canMoveIntoSquare(neighborNodes[i].Square, current.Square))
                    {
                        open.Add(current);
                    }
                }
            }

            int lowestFCost = open[0].FCost;
            int lowestIndex = 0;
            for(int i = 0; i < open.Count; i++)
            {
                if (open[i].FCost < lowestFCost)
                {
                    lowestFCost = open[i].FCost;
                    lowestIndex = i;
                }
            }

            current = open[lowestIndex];
            closed.Add(current);
            open.Remove(current);
        }

        while (current != null)
        { // LinkedList style
            pathList.Add(current);
            current = current.Previous;
        }
    }

    private bool canMoveIntoSquare(Square square, Square current)
    {
        if(square == null)
        {
            return false;
        }

        Vector2Int delta = square.GridPosition - current.GridPosition;    
        if (delta.y > 0)
        {
            return !square.SouthWall;
        }
        if (delta.x > 0)
        {
            return !square.WestWall;
        }
        if (delta.y < 0)
        {
            return !square.NorthWall;
        }
        if (delta.x < 0)
        {
            return !square.EastWall;
        }

        return false;
    }
}
