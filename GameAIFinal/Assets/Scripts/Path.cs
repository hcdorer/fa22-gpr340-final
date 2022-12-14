/* This game uses the A\* pathfinding algorithm, which is implemented based on pseudocode found in a YouTube video tutorial by Sebastian Lague.
 * The citation for the video can be found in the readme file at the root directory of this project.
 */

using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;

public class Path {
    public List<SquareNode> pathList { get; private set; }
    public int length { get => pathList.Count; }
    public bool isValid { get => pathList == null; }

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

    private bool canMoveIntoSquare(Square square, Square current) {
        if(square == null) {
            return false;
        }

        Vector2Int delta = square.GridPosition - current.GridPosition;
        if(delta.y > 0) {
            return !square.SouthWall;
        }
        if(delta.x > 0) {
            return !square.WestWall;
        }
        if(delta.y < 0) {
            return !square.NorthWall;
        }
        if(delta.x < 0) {
            return !square.EastWall;
        }

        return false;
    }
}
