/* This game uses the A* pathfinding algorithm, which is implemented based on pseudocode found in a YouTube video tutorial by Sebastian Lague.
 * https://www.youtube.com/watch?v=-L-WgKMFuhE
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNotValidException : Exception {
    public PathNotValidException() : base("This path is not valid.") { }
    public PathNotValidException(string message) : base(message) { }
    public PathNotValidException(string message, Exception inner) : base(message, inner) { }
}

public class Path {
    private List<SquareNode> pathList;
    public List<SquareNode> PathList { get => getPathList(); }
    public int length { get => getLength(); }
    public bool isValid { get => pathList != null; }
    public Square last { get => getLast(); }
    public Square this[int i] { get => getPathPoint(i); }
    
    public Path(Square origin, Square target) {
        pathList = null;
        makePath(origin, target);
    }

    private List<SquareNode> getPathList() {
        if(!isValid) {
            throw new PathNotValidException();
        }
        return pathList;
    }

    private int getLength() {
        if(!isValid) {
            throw new PathNotValidException();
        }
        return pathList.Count;
    }

    private Square getLast() {
        if(!isValid) {
            throw new PathNotValidException();
        }
        return pathList[length - 1].Square;
    }

    private Square getPathPoint(int i) {
        if(!isValid) {
            throw new PathNotValidException();
        }
        return pathList[i].Square;
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
            if(node == null) {
                continue;
            }

            updateMemberReferences(node, open);
            updateMemberReferences(node, closed);

            if(SquareNode.getNewGCost(current, node) < node.GCost) {
                node.Previous = current;
            }

            if(CharacterMovement.canMoveIntoSquare(node.Square, current.Square) && !closed.Contains(node)) {
                open.Add(node);
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
}
