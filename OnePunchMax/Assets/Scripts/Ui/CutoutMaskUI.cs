using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class CutoutMaskUI : Image
{
    private Material _material;

    public override Material materialForRendering
    {
        get
        {
            if (_material == null)
            {
                _material = new Material(base.materialForRendering);
                _material.SetInt("_StencilComp", (int)CompareFunction.NotEqual);
            }
            return _material;
        }
    }
}
