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
            updateOpen(current, open, closed);

            if(open.Count <= 0) {
                break;
            }

            current = getCheapestFCost(open);
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

    private static SquareNode getCheapestFCost(List<SquareNode> open) {
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

        return open[lowestIndex];
    }

    private void updateOpen(SquareNode current, List<SquareNode> open, List<SquareNode> closed) {
        SquareNode[] neighborNodes = current.getNeighborNodes();

        foreach(SquareNode node in neighborNodes) {
            if(node != null) {
                updateMemberReferences(node, open);
                updateMemberReferences(node, closed);
            }

            if(node != null) {
                if(SquareNode.getNewGCost(current, node) < node.GCost) {
                    node.Previous = current;
                }

                if(canMoveIntoSquare(node.Square, current.Square) && !closed.Contains(node)) {
                    open.Add(node);
                }
            }
        }
    }

    private static void updateMemberReferences(SquareNode node, List<SquareNode> list) {
        foreach(SquareNode listNode in list) {
            if(listNode.Equals(listNode)) {
                node = listNode;
            }
        }
    }

    private bool canMoveIntoSquare(Square next, Square current) {
        if(next == null) {
            return false;
        }

        Vector2Int delta = next.GridPosition - current.GridPosition;
        if(delta == Vector2.up) {
            return !next.SouthWall;
        }
        if(delta == Vector2.right) {
            return !next.WestWall;
        }
        if(delta == Vector2.down) {
            return !next.NorthWall;
        }
        if(delta == Vector2Int.left) {
            return !next.EastWall;
        }

        return false;
    }
}
