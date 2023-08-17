using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    // Attach this to the UnitManager object
    // This script is responsible for managing all the units in the level & tracking them at all times
    // This will serve as a hub for other scripts to have quick & easy access to any unit 
    // The UnitManager must execute before any other scripts for the proper order of operations
    public static UnitManager Instance {get; private set;} // Singleton Static Instance

    [SerializeField, Tooltip("List containing ALL units in the scene")]
    private List<Unit> _allUnitList;

    [SerializeField, Tooltip("List containing only FRIENDLY units in the scene")]
    private List<Unit> _friendlyUnitList;

    [SerializeField, Tooltip("List containing only ENEMY AI units in the scene")]
    private List<Unit> _enemyUnitList;


    private void Awake()
    {
        if(Instance != null) // If an instance of this script already exists before this one
        {
            Debug.LogError("Cannot have more than one UnitManager instance." + transform + "-" + Instance);
            Destroy(gameObject); // Destroy this duplicate instance
            return;
        }
        Instance = this;

        _allUnitList      = new List<Unit>();
        _friendlyUnitList = new List<Unit>();
        _enemyUnitList    = new List<Unit>();
    }
    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDeath += Unit_OnAnyUnitDeath;
    }

    public List<Unit> GetAllUnitList()
    {
        return _allUnitList;
    }

    public List<Unit> GetFriendlyUnitList()
    {
        return _friendlyUnitList;
    }

    public List<Unit> GetEnemyUnitList()
    {
        return _enemyUnitList;
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        // This event is fired from the Unit class, meaning sender is always of type Unit
        // Cache this unit, check its faction, and add it to the proper list

        Unit spawnedUnit = sender as Unit;

        if (spawnedUnit.IsEnemy())
        {
            _enemyUnitList.Add(spawnedUnit);
        } else _friendlyUnitList.Add(spawnedUnit);

        // Add this unit to this list regardless of faction
        _allUnitList.Add(spawnedUnit);
    }

    private void Unit_OnAnyUnitDeath(object sender, EventArgs e)
    {
        // This event is fired from the Unit class, meaning sender is always of type Unit
        // Cache this unit, check its faction, and remove it to the proper list

        Unit destroyedUnit = sender as Unit;

        if (destroyedUnit.IsEnemy())
        {
            _enemyUnitList.Remove(destroyedUnit);
        } else _friendlyUnitList.Add(destroyedUnit);

        // Add this unit to this list regardless of faction
        _allUnitList.Remove(destroyedUnit);
    }
}
