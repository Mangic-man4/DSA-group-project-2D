using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathNode2d
{ 
    private Grid2d<PathNode2d> grid;
    public int x;
    public int y;

    public int gCost;
    public int hCost;
    public int fCost;

    public PathNode2d cameFromNode;
    public PathNode2d(Grid2d<PathNode2d> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y; 
    }

    public void CalculateFCost()
    {
        fCost = gCost + hCost;
    }

    public override string ToString()
    {
        return x + ", " + y;
    }   
}
