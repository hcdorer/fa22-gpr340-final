/* This game uses the A* pathfinding algorithm, which is implemented based on pseudocode found in a YouTube video tutorial by Sebastian Lague.
 * https://www.youtube.com/watch?v=-L-WgKMFuhE
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {
    public List<SquareNode> pathList { get; private set; }
    public int length { get => pathList.Count; }
    public bool isValid { get => pathList != null; }
    public Square last { get => pathList[length - 1].Square; }

    public Path(Square origin, Square target) {
        pathList = null;
        makePath(origin, target);
    }

    private void makePath(Square origin, Square target) {
        List<SquareNode> open = new List<SquareNode>();
        List<SquareNode> closed = new List<SquareNode>();

        SquareNode current = new SquareNode(origin, target);
        closed.Add(current);

        while(current.Square != target) {
            SquareNode[] neighborNodes = current.getNeighborNodes();
            bool[] alreadyExists = new bool[neighborNodes.Length];

            for(int i = 0; i < neighborNodes.Length; i++) {
                if(neighborNodes[i] != null) {
                    foreach(SquareNode n in open) {
                        if(neighborNodes[i].Equals(n)) {
                            neighborNodes[i] = n;
                            alreadyExists[i] = true;
                        }
                    }

                    foreach(SquareNode n in closed) {
                        if(neighborNodes[i].Equals(n)) {
                            neighborNodes[i] = n;
                            alreadyExists[i] = true;
                        }
                    }
                }
            }

            foreach(SquareNode neighbor in neighborNodes) {
                if(neighbor != null) {
                    if(SquareNode.getNewGCost(current, neighbor) < neighbor.GCost) {
                        neighbor.Previous = current;
                    }

                    if(canMoveIntoSquare(neighbor.Square, current.Square) && !closed.Contains(neighbor)) {
                        open.Add(neighbor);
                    }
                }
            }

            if(open.Count <= 0) {
                break;
            }

            int lowestFCost = open[0].FCost;
            int lowestIndex = 0;
            for(int i = 0; i < open.Count; i++) {
                if(open[i].FCost < lowestFCost) {
                    lowestFCost = open[i].FCost;
                    lowestIndex = i;
                } else if(open[i].FCost == lowestFCost && open[i].HCost < open[lowestIndex].HCost) {
                    lowestIndex = i;
                }
            }

            current = open[lowestIndex];
            closed.Add(current);
            open.Remove(current);
        }

        if(current.Square == target) {
            pathList = new List<SquareNode>();
            while(current != null) { // LinkedList style
                pathList.Insert(0, current);
                current = current.Previous;
            }
        }
    }

    private bool canMoveIntoSquare(Square next, Square current) {
        if(next == null) {
            return false;
        }

        Vector2Int delta = next.GridPosition - current.GridPosition;
        if(delta.y > 0) {
            return !next.SouthWall;
        }
        if(delta.x > 0) {
            return !next.WestWall;
        }
        if(delta.y < 0) {
            return !next.NorthWall;
        }
        if(delta.x < 0) {
            return !next.EastWall;
        }

        return false;
    }
}
