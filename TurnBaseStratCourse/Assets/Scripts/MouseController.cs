using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseController : MonoBehaviour
{   
    private static MouseController _instance;

    [SerializeField, Tooltip("The layer the on which mouse registers input, which should only be the ground")]
    private LayerMask mousePlaneMask;

    void Awake() 
    {
        _instance = this;
    }
    
    public static Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, _instance.mousePlaneMask);
        return hit.point;
    }
}
