using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystemVisualSingle : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _mRenderer;

    public void Show(Material material)
    {
        _mRenderer.enabled = true;
        _mRenderer.material = material;
    }

    public void Hide()
    {
        _mRenderer.enabled = false;
    }
}
