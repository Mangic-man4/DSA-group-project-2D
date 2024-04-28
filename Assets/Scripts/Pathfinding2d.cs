using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding2d
{
    // Cost of moving straight and diagonally
    private const int MOVE_STRAIGHT_COST = 10;
    private const int MOVE_DIAGONAL_COST = 14;

    public static Pathfinding2d Instance { get; private set; } // Singleton instance


    private Grid2d<PathNode2d> grid; // Grid for pathfinding
    private List<PathNode2d> openList; // List of nodes to be evaluated
    private List<PathNode2d> closedList; // List of nodes that have been evaluated

    public Pathfinding2d(int width, int height)
    {
        Instance = this;
        grid = new Grid2d<PathNode2d>(width, height, 10f, Vector3.zero, (Grid2d<PathNode2d> g, int x, int y) => new PathNode2d(g, x, y));
    }

    public Grid2d<PathNode2d> GetGrid() // Get the grid for external use
    {
        return grid;
    }

    public List<Vector3> FindPath(Vector3 startWorldPosition, Vector3 endWorldPosition) // Find a path from start to end world position
    {
        grid.GetXY(startWorldPosition, out int startX, out int startY);
        grid.GetXY(endWorldPosition, out int endX, out int endY);
        
        List<PathNode2d> path = FindPath(startX, startY, endX, endY);
        if (path == null)
        {
            return null;
        }
        else
        {
            List<Vector3> vectorPath = new List<Vector3>();
            foreach (PathNode2d pathNode in path)
            {
                vectorPath.Add(new Vector3(pathNode.x, pathNode.y) * grid.GetCellSize() + Vector3.one * grid.GetCellSize() * .5f);
            }
            return vectorPath;
        }
    }

    public List<PathNode2d> FindPath(int startX, int startY, int endX, int endY) // Find a path from (startX, startY) to (endX, endY)
    {
        PathNode2d startNode = grid.GetGridObject(startX, startY);
        PathNode2d endNode = grid.GetGridObject(endX, endY);

        openList = new List<PathNode2d> { startNode };
        closedList = new List<PathNode2d>();

        for (int x = 0; x < grid.GetWidth(); x++) // Reset costs and references for all nodes
        {
            for (int y = 0; y < grid.GetHeight(); y++)
            {
                PathNode2d pathNode = grid.GetGridObject(x, y);
                pathNode.gCost = int.MaxValue;
                pathNode.CalculateFCost();
                pathNode.cameFromNode = null;
            }
        }

        // Set initial costs for the start node
        startNode.gCost = 0;
        startNode.hCost = CalculateDistanceCost(startNode, endNode);
        startNode.CalculateFCost();

        while(openList.Count > 0) // Loop until openList is empty
        {
            PathNode2d currentNode = GetLowestFCostNode(openList);
            if (currentNode == endNode)
            {
                // Reached final node
                return CalculatePath(endNode);
            }
            openList.Remove(currentNode);
            closedList.Add(currentNode);

            foreach (PathNode2d neighbourNode in GetNeighbourList(currentNode))
            {
                if (closedList.Contains(neighbourNode)) continue;
                if (!neighbourNode.isWalkable)
                {
                    closedList.Add(neighbourNode);
                    continue;
                }
                int tentativeGCost = currentNode.gCost + CalculateDistanceCost(currentNode, neighbourNode);
                if (tentativeGCost < neighbourNode.gCost)
                {
                    neighbourNode.cameFromNode = currentNode;
                    neighbourNode.gCost = tentativeGCost;
                    neighbourNode.hCost = CalculateDistanceCost(neighbourNode, endNode);
                    neighbourNode.CalculateFCost();

                    if (!openList.Contains(neighbourNode))
                    {
                        openList.Add(neighbourNode);
                    }
                }
            }
        }

        // Out of nodes on the openList
        return null;

    }

    private List<PathNode2d> GetNeighbourList(PathNode2d currentNode) // Get list of neighbouring nodes
    {
        List<PathNode2d> neighbourList = new List<PathNode2d>();
        if (currentNode.x - 1 >= 0)
        {
            // Left
            neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y));
            // Left Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y - 1));
            // Left Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x - 1, currentNode.y + 1));
        }

        if (currentNode.x + 1 < grid.GetWidth())
        {
            // Right
            neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y));
            // Right Down
            if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y - 1));
            // Right Up
            if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x + 1, currentNode.y + 1));
        }

        // Down
        if (currentNode.y - 1 >= 0) neighbourList.Add(GetNode(currentNode.x, currentNode.y - 1));
        // Up
        if (currentNode.y + 1 < grid.GetHeight()) neighbourList.Add(GetNode(currentNode.x, currentNode.y + 1));

        return neighbourList;

}

    public PathNode2d GetNode(int x, int y) // Get node at given coordinates
    {
        return grid.GetGridObject(x, y);
    }

    private List<PathNode2d> CalculatePath(PathNode2d endNode) // Calculate the path from endNode to startNode
    {
        List<PathNode2d> path = new List<PathNode2d>();
        path.Add(endNode);
        PathNode2d currentNode = endNode;
        while (currentNode.cameFromNode != null)
        {
            path.Add(currentNode.cameFromNode);
            currentNode = currentNode.cameFromNode;
        }
        path.Reverse();
        return path;
    }

    private int CalculateDistanceCost(PathNode2d a, PathNode2d b) // Calculate the distance cost between two nodes
    {
        int xDistance = Mathf.Abs(a.x - b.x);
        int yDistance = Mathf.Abs(a.y - b.y);
        int remaining = Mathf.Abs(xDistance - yDistance);
        return MOVE_DIAGONAL_COST * Mathf.Min(xDistance, yDistance) + MOVE_STRAIGHT_COST * remaining;
    }

    private PathNode2d GetLowestFCostNode(List<PathNode2d> pathNodeList) // Get the node with the lowest F cost from a list of nodes
    {
        PathNode2d lowestFCostNode = pathNodeList[0];
        for (int i = 1; i < pathNodeList.Count; i++)
        {
            if (pathNodeList[i].fCost < lowestFCostNode.fCost)
            {
                lowestFCostNode = pathNodeList[i];
            }
        }

        return lowestFCostNode;

    }
}
