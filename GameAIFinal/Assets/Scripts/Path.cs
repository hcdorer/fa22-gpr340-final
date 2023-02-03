/* This game uses the A* pathfinding algorithm, which is implemented based on pseudocode found in a YouTube video tutorial by Sebastian Lague.
 * https://www.youtube.com/watch?v=-L-WgKMFuhE
 */

using System;
using System.Collections.Generic;

public class PathNotValidException : Exception {
    public PathNotValidException() : base("This path is not valid.") { }
    public PathNotValidException(string message) : base(message) { }
    public PathNotValidException(string message, Exception inner) : base(message, inner) { }
}

public class Path {
    private List<SquareNode> pathList;
    private List<SquareNode> PathList { get => getPathList(); }
    public int length { get => PathList.Count; }
    public bool isValid { get => pathList != null; }
    public Square last { get => PathList[length - 1].Square; }
    public Square this[int i] { get => PathList[i].Square; }
    
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

        foreach(SquareNode neighbor in neighborNodes) {
            if(neighbor == null) {
                continue;
            }

            updateReferences(neighbor, open);
            updateReferences(neighbor, closed);

            if(SquareNode.getNewGCost(current, neighbor) < neighbor.GCost) {
                neighbor.Previous = current;
            }

            if(CharacterMovement.canMoveIntoSquare(neighbor.Square, current.Square) && !closed.Contains(neighbor)) {
                open.Add(neighbor);
            }
        }
    }

    private static void updateReferences(SquareNode node, List<SquareNode> list) {
        foreach(SquareNode listNode in list) {
            if(listNode.Equals(listNode)) {
                node = listNode;
            }
        }
    }
}
