using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem // Does not inherit from MonoBehaviour because we want to use a constructor to instantiate this, and it won't be attaached to a GameObject
{
    private int           _width;           // The total # of cells in the grid in the X direction
    private int           _height;          // The total # of cells in the grid in the Z direction
    private float         _cellSize;        // The size in world units of each cell in the grid
    private GridObject[,] _gridObjectArray; // 2D Array to store each GridObject in the GridSystem

    public GridSystem(int width, int height, float cellSize) // Constructor
    {
        _width = width;
        _height = height;
        _cellSize = cellSize;

        _gridObjectArray = new GridObject[width, height]; // Initialize Array of GridObjects configured to the width & height of this GridSystem

        for (int x = 0; x < _width; x++) 
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);           // Create a new GridPosition for this particiular cell of the GridSystem
                _gridObjectArray[x, z] =  new GridObject(this, gridPosition); // Create a new GridObject at this position, and add that object to the GridObjectArray
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition) 
    {
        // Takes a GridPosition within the GridSystem & returns the real world coordinates of it
        return new Vector3(gridPosition.x, 0, gridPosition.z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize),
            Mathf.RoundToInt(worldPosition.z /_cellSize)
        );
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++) 
        {
            for (int z = 0; z < _height; z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

               Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity); // Instantiate the DebugObject in position of its cell & cache it
               GridDebugObject gridDebugObject =  debugTransform.GetComponent<GridDebugObject>(); // Cache the GridDebugObject component on the newly made DebugObject
              gridDebugObject.SetGridObject(GetGridObject(gridPosition));  // Have this DebugObject set itself up
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        // Returns the GridObject at a given GridPosition in the GridSystem
        return _gridObjectArray[gridPosition.x, gridPosition.z];
    }

    public bool isValidGridPosition(GridPosition gridPosition)
    {
        // Takes in a GridPosition and returns whether it is valid or not, meaning it is a GridPosition that actually
        // exists within this GridSystem

        return gridPosition.x >= 0 && // The GridSystem originates from (0,0) so it will never have a (-) x or z GridPosition
               gridPosition.z >= 0 &&
               gridPosition.x < _width && // Checks if the GridPosition isn't out of bounds of the GridSystem in the (+) direction
               gridPosition.z < _height;
    }
}
