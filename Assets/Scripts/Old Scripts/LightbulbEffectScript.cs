using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class LightbulbEffectScript : MonoBehaviour
{

    public static LightbulbEffectScript lightbulbEffectScript;
    public Light pLight;

    public Material offMat;
    public Material onMat;

    public MeshRenderer ren;

    private void Awake()
    {
        lightbulbEffectScript = this;
        ren.material = offMat;
    }

    public void LevelComplete()
    {
        pLight.enabled = true;
        ren.material = onMat;
    }


}
