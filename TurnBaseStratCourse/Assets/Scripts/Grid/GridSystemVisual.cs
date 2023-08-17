using System;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisual : MonoBehaviour
{
    public enum GridVisualType
    {
        Blue,
        Red, 
        RedSoft,
        White,
        Yellow,
    }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVisualType;
        public Material        material;

    }

    public static GridSystemVisual Instance { get; private set; }

    [SerializeField, Tooltip("Prefab for the visual of the grid")]
    private Transform _gridSystemVisualSingle;

    [SerializeField, Tooltip("A list of a custom stuct conttaining an enum for the type of color & corresponding material")]
    private List<GridVisualTypeMaterial> _GridVisualTypeMaterialList;

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
        UnitActionSystem.Instance.OnSelectedActionChange += UnitActonSystem_OnSelectedActionChange;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;

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

        UpdateGridVisual();
    }

    private void UnitActonSystem_OnSelectedActionChange(object sender, EventArgs e)
    {
        UpdateGridVisual();
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e)
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

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
    {
        // Takes in a GridPosition, a range & GridVisual type
        // Shows the valid GridPositions within the given range from the given gridPosition
        // in the color specified by the given GridVisualType

        List<GridPosition> gridPositionList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range;  z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                // Ignore this GridPosition if it is not an actual position on the GridSystem
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition)) continue;

                // Calculate the Testing Distance for valid GridPositions of this object
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);
                // Ignore this grid position if it is out of range
                if (testDistance > range) continue;

                gridPositionList.Add(testGridPosition);
            }
        }
        // Take the valid list of GridPositions in range of the given action and show all their visuals
        ShowGridPositionList(gridPositionList, gridVisualType);
    }

    public void ShowGridPositionList(List<GridPosition> gridPositionList, GridVisualType gridVisualType)
    {
        // Takes in a list of GridPositions and toggles on their visuals
        foreach (GridPosition gridPosition in gridPositionList)
        {
            _gridSystemVisualSingleArray[gridPosition.x, gridPosition.z].Show(GetGridVisualTypeMaterial(gridVisualType));
        }
    }

    private void UpdateGridVisual()
    {
        HideAllGridVisuals();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;
        switch (selectedBaseAction)
        {
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;

                // Show all valid GridPositions within the range of the ShootACtion in a Soft Red color
                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootRange(), GridVisualType.RedSoft);
                break;
            default:
                gridVisualType = GridVisualType.White;
                break;
        }

        ShowGridPositionList(selectedBaseAction.GetValidActionGridPositionList(), gridVisualType); // Show all valid GridPositions for the currently selected action
    }

    private Material GetGridVisualTypeMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in _GridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVisualType == gridVisualType)
            {
                return gridVisualTypeMaterial.material;
            }
        }

        Debug.LogError("Could not find GridVisualTypeMaterial for GridVisualType " + gridVisualType);
        return null;

    }
}
