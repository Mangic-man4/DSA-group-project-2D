using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class PathTesting : MonoBehaviour
{
    [SerializeField] private PathfindingVisual pathfindingvisual;
    [SerializeField] private CharacterPathfindingMovementHandler characterPathfinding;
    [SerializeField] private PathfindingDebugStepVisual pathfindingDebugStepVisual; //can be removed

    private Pathfinding2d pathfinding; // Pathfinding algorithm instance

    private void Start()
    {
       pathfinding = new Pathfinding2d(10, 10); //Create a grid of 10x10
       pathfindingvisual.SetGrid(pathfinding.GetGrid()); // Setting up the grid visualization
        pathfindingDebugStepVisual.Setup(pathfinding.GetGrid()); //can be removed

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Left click
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition(); //Get the mouse position in the world
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y); //Convert world position to grid position
            List<PathNode2d> path = pathfinding.FindPath(0, 0, x, y); //Find a path from 0,0 to the clicked grid position
            if (path != null)
            {
                for (int i = 0; i < path.Count - 1; i++) // Draw lines to visualize the path

                {
                    Debug.DrawLine(new Vector3(path[i].x, path[i].y) * 10f + Vector3.one * 5f, new Vector3(path[i + 1].x, path[i + 1].y) * 10f + Vector3.one * 5f, Color.green, 5f);
                }
            }
            characterPathfinding.SetTargetPosition(mouseWorldPosition); //move character to the clicked position
        }
        if (Input.GetMouseButtonDown(1)) //Right click
        {
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition(); //Get the mouse position in the world
            pathfinding.GetGrid().GetXY(mouseWorldPosition, out int x, out int y); //Convert world position to grid position
            pathfinding.GetNode(x, y).SetIsWalkable(!pathfinding.GetNode(x, y).isWalkable); //Toggle walkable state of the clicked node
        }
    }
}
