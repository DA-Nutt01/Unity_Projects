using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridTester : MonoBehaviour
{
    [SerializeField]  private Unit _unit;

    void Start()
    {
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        {
            _unit.GetMoveAction().GetValidActionGridPositionList();
        }
    }

}
