using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerMask : MonoBehaviour
{

    public static int CreateLayerMask(bool aExclude, params int[] aLayers)
    {
        int v = 0;
        foreach (var L in aLayers)
            v |= 1 << L;
        if (aExclude)
            v = ~v;
        return v;
    }
}
