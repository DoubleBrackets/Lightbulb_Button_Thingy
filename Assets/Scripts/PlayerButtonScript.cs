using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonScript : MonoBehaviour
{
    public static PlayerButtonScript playerButtonScript;

    public MeshRenderer ren;

    public MeshRenderer lightbulbMat;

    private float emissionMinIntensity = -2;
    private float emissionmaxIntensity = 2.5f;

    public Color onColor;
    private Color offColor;

    Rigidbody rb;

    public Light bulbLight;

    private float minForce = 1f;
    private float winForce = 50f;

    private float minIntensity = 0f;
    private float maxIntensity = 7f;

    public bool isFlashing = false;

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        isFlashing = true;
        ren.material.SetColor("_EmissionColor", onColor);

        Vector3 localPos = ren.transform.localPosition;
        localPos.y += 0.07f;
        ren.transform.localPosition = localPos;
        float ratio = Mathf.Min(1f, (force / winForce));

        Color c = lightbulbMat.material.GetColor("_EmissionColor");
        c *= 10;// emissionMinIntensity + ratio * (emissionmaxIntensity - emissionMinIntensity);
        lightbulbMat.material.SetColor("_EmissionColor", c);

        for(int x = 0;x <= 30;x++)
        {
            bulbLight.intensity = minIntensity + (1 - x / 30f) * ratio * (maxIntensity-minIntensity);
            yield return new WaitForFixedUpdate();
        }

        c *= 0;// emissionMinIntensity + ratio * (emissionmaxIntensity - emissionMinIntensity);
        lightbulbMat.material.SetColor("_EmissionColor", c);

        bulbLight.intensity = 0;
        isFlashing = false;
        ren.material.SetColor("_EmissionColor", offColor);
        localPos.y -= 0.07f;
        ren.transform.localPosition = localPos;
    }
}
