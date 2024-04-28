using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode2d
{ 
    private Grid2d<PathNode2d> grid;
    public int x;
    public int y;

    // Costs for pathfinding algorithm
    public int gCost;
    public int hCost;
    public int fCost; //Total cost

    public bool isWalkable;
    public PathNode2d cameFromNode;
    public PathNode2d(Grid2d<PathNode2d> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y; 
        isWalkable = true; //Default value = true
    }

    public void CalculateFCost() // Calculate the total cost of the node

    {
        fCost = gCost + hCost;
    }

    public void SetIsWalkable(bool isWalkable) // Set the walkable state of the node
    {
        this.isWalkable = isWalkable;
        grid.TriggerGridObjectChanged(x, y);
    }

    public override string ToString() // Override ToString method to provide a string representation of the node
    {
        return x + ", " + y;
    }   
}
