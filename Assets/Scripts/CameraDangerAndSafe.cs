using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
public class CameraDangerAndSafe : MonoBehaviour
{
    public Vignette vignette;
    bool danger;
    void Start()
    {
        this.GetComponent<PostProcessVolume>().profile.TryGetSettings<Vignette>(out vignette);
    }

    void Update()
    {
        if (danger)
        {
            vignette.color.value = Color.red;
        }
        else
        {
            vignette.color.value = Color.black;
        }
    }
}
