using CodeMonkey.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid2d<TGridObject>
{

    //public const int HEAT_MAP_MAX_VALUE = 100;
    //public const int HEAT_MAP_MIN_VALUE = 0;


    public event EventHandler<OnGridObjectChangedEventArgs> OnGridObjectChanged; // Event triggered when a grid object changes 
    public class OnGridObjectChangedEventArgs : EventArgs
    {
        public int x;
        public int y;
    }

    // Grid properties
    private int width;
    private int height;
    private float cellSize;
    private Vector3 originPosition;
    private TGridObject[,] gridArray;

    public Grid2d(int width, int height, float cellSize, Vector3 originPosition, Func<Grid2d<TGridObject>, int, int, TGridObject> createGridObject)
    {
        this.width = width;
        this.height = height;
        this.cellSize = cellSize;
        this.originPosition = originPosition;

        gridArray = new TGridObject[width, height];

        for (int x = 0; x < gridArray.GetLength(0); x++) // Populate grid with grid objects
        {
            for (int y = 0; y < gridArray.GetLength(1); y++)
            {
                gridArray[x, y] = createGridObject(this, x, y);
            }
        }

        bool showDebug = true; // Debug visualization
        if (showDebug)
        {
            TextMesh[,] debugTextArray = new TextMesh[width, height];


            for (int x = 0; x < gridArray.GetLength(0); x++)
            {
                for (int y = 0; y < gridArray.GetLength(1); y++)
                {
                    Debug.Log(gridArray[x, y]);
                    debugTextArray[x, y] = UtilsClass.CreateWorldText(gridArray[x, y]?.ToString(), null, GetWorldPosition(x, y) + new Vector3(cellSize, cellSize) * .5f, 30, Color.white, TextAnchor.MiddleCenter);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x, y + 1), Color.white, 100f);
                    Debug.DrawLine(GetWorldPosition(x, y), GetWorldPosition(x + 1, y), Color.white, 100f);
                }
            }
            Debug.DrawLine(GetWorldPosition(0, height), GetWorldPosition(width, height), Color.white, 100f);
            Debug.DrawLine(GetWorldPosition(width, 0), GetWorldPosition(width, height), Color.white, 100f);

            OnGridObjectChanged += (object sender, OnGridObjectChangedEventArgs eventArgs) => // Update debug text when grid object changes
            {
                debugTextArray[eventArgs.x, eventArgs.y].text = gridArray[eventArgs.x, eventArgs.y]?.ToString();
            };
        }
    }

    public int GetWidth() // Get grid width
    {
        return width;
    }

    public int GetHeight() // Get grid height
    {
        return height;
    }

    public float GetCellSize() // Get cell size
    {
        return cellSize;
    }

    public Vector3 GetWorldPosition(int x, int y) // Convert grid coordinates to world position
    {
        return new Vector3(x, y) * cellSize + originPosition;
    }

    public void GetXY(Vector3 worldPosition, out int x, out int y) // Convert world position to grid coordinates
    {
        x = Mathf.FloorToInt((worldPosition - originPosition).x / cellSize);
        y = Mathf.FloorToInt((worldPosition - originPosition).y / cellSize);
    }

    public void SetGridObject(int x, int y, TGridObject value) // Set grid object at specified coordinates
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            gridArray[x, y] = value;
            // Trigger event
            if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
        }
    }

    public void SetGridObject(Vector3 worldPosition, TGridObject value) // Set grid object at specified world position
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        SetGridObject(x, y, value);
    }

    public void TriggerGridObjectChanged(int x, int y) // Trigger event when a grid object changes
    {
        if (OnGridObjectChanged != null) OnGridObjectChanged(this, new OnGridObjectChangedEventArgs { x = x, y = y });
    }

    public TGridObject GetGridObject(int x, int y) // Get grid object at specified coordinates
    {
        if (x >= 0 && y >= 0 && x < width && y < height)
        {
            return gridArray[x, y];
        }
        else
        {
            return default(TGridObject);
        }
    }
    public TGridObject GetGridObject(Vector3 worldPosition) // Get grid object at specified world position
    {
        int x, y;
        GetXY(worldPosition, out x, out y);
        return GetGridObject(x, y);
    }
}