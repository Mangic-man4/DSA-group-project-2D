using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class Testing2d : MonoBehaviour
{
    //private Grid2d<int> grid;
    //private List<Grid2d<int>> grids = new List<Grid2d<int>>();
    private Grid2d<HeatMapGridObject> grid; // Grid for heatmap visualization
    private Grid2d<bool> boolGrid; // Grid for boolean values
    private Grid2d<StringGridObject> stringGrid; // Grid for string values

    void Start()
    {
        // Initialize heatmap grid
        grid = new Grid2d<HeatMapGridObject>(20, 10, 8f, Vector3.zero, (Grid2d<HeatMapGridObject> g, int x, int y) => new HeatMapGridObject(g, x, y));
        // Initialize boolean grid
        //boolGrid = new Grid<bool>(20, 10, 8f, Vector3.zero, (Grid<bool> g, int x, int y) => false);
        // Initialize string grid
        //stringGrid = new Grid<StringGridObject>(20, 10, 8f, Vector3.zero, (Grid<StringGridObject> g, int x, int y) => new StringGridObject(g, x, y));

        /*
        grids.Add(new Grid2d<int>(4, 2, 10f, new Vector3(-25, 0)));
        grids.Add(new Grid2d<int>(3, 4, 10f, new Vector3(20, 15)));
        grids.Add(new Grid2d<int>(2, 2, 20f, new Vector3(25, -20)));*/

        /*
        grid = new Grid2d(4, 2, 10f, new Vector3(-25, 0));
        grid = new Grid2d(3, 4, 10f, new Vector3(20, 15)); 
        new Grid2d(2, 2, 20f, new Vector3(25, -20)); */
    }

    void Update()
    {
        Vector3 position = UtilsClass.GetMouseWorldPosition();

        if (Input.GetMouseButtonDown(0)) // Left click
        {
            HeatMapGridObject heatMapGridObject = grid.GetGridObject(position); // Add value to the heatmap grid at the mouse position
            if (heatMapGridObject != null)
            {
                heatMapGridObject.AddValue(5);

                /*foreach (Grid2d<int> grid in grids)
            {
                grid.SetGridObject(UtilsClass.GetMouseWorldPosition(), 56);
            }*/
        }

        if (Input.GetMouseButtonDown(1)) // Right click
        {
                /*
            // Get the mouse world position
            Vector3 mouseWorldPosition = UtilsClass.GetMouseWorldPosition();

            // Find the grid at the mouse position
            Grid2d<int> gridAtMousePosition = grids.Find(grid => grid.GetGridObject(mouseWorldPosition) != 0);

            // Check if a grid was found
            if (gridAtMousePosition != null)
            {
                // Output the value of the grid at the mouse position
                Debug.Log("Value: " + gridAtMousePosition.GetGridObject(mouseWorldPosition));
            }
            else
            {
                Debug.Log("Value: 0");*/
            }
        }
    }
}
public class HeatMapGridObject // Class representing a grid object for heatmap visualization
{

    private const int MIN = 0;
    private const int MAX = 100;

    private Grid2d<HeatMapGridObject> grid;
    private int x;
    private int y;
    private int value;

    public HeatMapGridObject(Grid2d<HeatMapGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
    }

    public void AddValue(int addValue)     // Add a value to the grid object

    {
        value += addValue;
        value = Mathf.Clamp(value, MIN, MAX);
        grid.TriggerGridObjectChanged(x, y);
    }

    public float GetValueNormalized()     // Get the normalized value of the grid object

    {
        return (float)value / MAX;
    }

    public override string ToString()     // Override ToString method to provide a string representation of the grid object

    {
        return value.ToString();
    }

}

public class StringGridObject // Class representing a grid object for string visualization
{

    private Grid2d<StringGridObject> grid;
    private int x;
    private int y;

    private string letters;
    private string numbers;

    public StringGridObject(Grid2d<StringGridObject> grid, int x, int y)
    {
        this.grid = grid;
        this.x = x;
        this.y = y;
        letters = "";
        numbers = "";
    }

    public void AddLetter(string letter)     // Add a letter to the grid object

    {
        letters += letter;
        grid.TriggerGridObjectChanged(x, y);
    }

    public void AddNumber(string number)     // Add a number to the grid object

    {
        numbers += number;
        grid.TriggerGridObjectChanged(x, y);     
    }

    public override string ToString() // Override ToString method to provide a string representation of the grid object
    {
        return letters + "\n" + numbers;
    }

}

