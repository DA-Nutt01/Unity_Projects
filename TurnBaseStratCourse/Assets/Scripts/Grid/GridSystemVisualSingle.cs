using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _mRenderer;

    public void Show()
    {
        _mRenderer.enabled = true;
    }

    public void Hide()
    {
        _mRenderer.enabled = false;
    }
}
