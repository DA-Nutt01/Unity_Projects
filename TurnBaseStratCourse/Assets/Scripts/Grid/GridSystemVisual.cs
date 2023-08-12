using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField, Tooltip("Prefab for the visual of the grid")]
    private Transform _gridSystemVisualSingle;

    private GridSystemVisualSingle[,] _gridSystemVisualSingleArray; // 2D Array

    void Awake()
    {
        if (Instance != null) // If an instance of this script already exists before this one
        {
            Debug.LogError("Cannot have more than one GridSsytemVisual instance!" + transform + "-" + Instance);
            Destroy(gameObject); // Destroy this duplicate instance
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        _gridSystemVisualSingleArray = new GridSystemVisualSingle[
            LevelGrid.Instance.GetWidth(),
            LevelGrid.Instance.GetHeight()
        ];

        // For every GridPosition in the GridSystem
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++) 
            { 
                GridPosition gridPostion = new GridPosition(x, z); // Temp Gridposition for current iterated GridPosition in System

               Transform gridSystemVisualSingleTransform = Instantiate(_gridSystemVisualSingle, LevelGrid.Instance.GetWorldPosition(gridPostion), Quaternion.identity);

                _gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
            }
        }
    }

    private void Update()
    {
        UpdateGridVisual();
    }

    public void HideAllGridVisuals()
    {
        // Iterate through every GridPosition in the GridSystem
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetHeight(); z++)
            {
                // Access the array of GridVisualSingles and hide the visual at this index
                _gridSystemVisualSingleArray[x, z].Hide();
            }
        }
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList)
    {
        // Takes in a list of GridPositions and toggles on their visuals
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show();
        }
    }

    private void UpdateGridVisual()
    {
        //
        HideAllGridVisuals();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        ShowGridPositionList(selectedUnit.GetMoveAction().GetValidActionGridPositionList());
    }
}
