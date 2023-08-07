using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GridDebugObject : MonoBehaviour
{
    [SerializeField, Tooltip("A reference to the TMPro child text object")]
    private TextMeshPro _textObject;

    [SerializeField, Tooltip("The GridObject this is attached to")]
    private GridObject _gridObject;

    public void SetGridObject(GridObject gridObject) 
    {
        _gridObject = gridObject;
    }

    void Update() 
    {
        _textObject.text = _gridObject.ToString();
    }
}
