using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEditor.EditorTools;
using UnityEngine;
public class ShootAction : BaseAction
{
    private enum State 
    {
        Aiming,
        Shooting, 
        Cooloff,
    }

    [SerializeField, Tooltip("The max number of tiles this unit can shoot a target from")] 
    private int   _maxShootRange = 7;

    [SerializeField, Tooltip("Multiplier for rotating speed when unit is turning")]
    private float _rotateSpeed = 15f;

    public event EventHandler<OnShootEventArgs> OnShoot;
    public class OnShootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    private State _state;
    private float _stateTimer; // Amount of time between switching to each state of this action
    private Unit  _targetUnit;
    private bool  _canShoot;

    private void Update()
    {
        if (!_isActive) return; // Skip all other code while this action is not active
    
        _stateTimer -= Time.deltaTime;
        switch(_state)
        {
            case State.Aiming:
                Vector3 aimDirection = (_targetUnit.GetWorldPosition() - _unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDirection, Time.deltaTime * _rotateSpeed);
                break;
            case State.Shooting:
                if (_canShoot)
                {
                    Shoot();
                    _canShoot = false;
                } 
                break;
            case State.Cooloff:
                break;
        }

        if (_stateTimer <= 0f) NextState();
    }

    private void NextState()
    {
        switch (_state)
        {
            case State.Aiming:
                _state = State.Shooting;
                float shootStateTime = 0.1f; // The amount of time spent in this state
                _stateTimer = shootStateTime;
                break;
            case State.Shooting:
                _state = State.Cooloff;
                float cooloffStatetime = 0.5f; // The amount of time spent in this state
                _stateTimer = cooloffStatetime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnShootEventArgs{targetUnit = _targetUnit, shootingUnit = _unit});
        // Randomize the damage for every shot
        int shootDamage = UnityEngine.Random.Range(20, 60);
        _targetUnit.TakeDamage(shootDamage);
    }
    public override string GetActionName()
    {
        return "Shoot";
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        // Returns a list of  valid GridPositions that are within this unit's max move distance
        List<GridPosition> validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = _unit.GetGridPosition();

        for (int x = -_maxShootRange; x <= _maxShootRange; x++)
        {
            for (int z = -_maxShootRange; z <= _maxShootRange; z++) 
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                // Ignore this GridPosition if it is not an actual position on the GridSystem
                if (!LevelGrid.Instance.isValidGridPosition(testGridPosition)) continue; 

                // Calculate the Testing Distance for valid GridPositions of this object
                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > _maxShootRange) continue;

                // Ignore this GridPosition if it does NOT contain a unit
                if (!LevelGrid.Instance.isGridPositionOccupied(testGridPosition)) continue; 

                // Cache the unit on this given GridPosition
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                // Ignore this GridPositon if the unit occupying it is the same team as this unit
                if (targetUnit.IsEnemy() == _unit.IsEnemy()) continue; 

                // One this testGridPosition passes all the checks, add it to the list of ValidGridPositions
                validGridPositionList.Add(testGridPosition);
            }
        }

        return validGridPositionList;
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        
        _targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        _state = State.Aiming;
        float aimStateTime = 1f; // The amount of time spent in this state
        _stateTimer = aimStateTime;
        _canShoot = true;

        // For execution order purposes, make sure ActionStart() is called after initial setup of this action
        ActionStart(onActionComplete);
    }

    public Unit GetTargetUnit()
    {
        return _targetUnit;
    }

    public int GetShootRange()
    {
        return _maxShootRange;
    }
}
