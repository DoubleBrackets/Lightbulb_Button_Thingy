using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonScript : MonoBehaviour
{
    public static PlayerButtonScript playerButtonScript;

    public MeshRenderer ren;

    public MeshRenderer lightbulbMat;

    private float emissionMinIntensity = 0.4f;
    private float emissionmaxIntensity = 10f;

    private Color lightbulbMatColor;

    public Color onColor;
    private Color offColor;

    Rigidbody rb;

    public Light bulbLight;

    private float minForce = 3f;
    private float winForce = 50f;

    private float minIntensity = 0f;
    private float maxIntensity = 7f;

    public bool isFlashing = false;

    private void Awake()
    {
        playerButtonScript = this;
        rb = gameObject.GetComponentInParent<Rigidbody>();
        lightbulbMatColor = lightbulbMat.material.GetColor("_EmissionColor");
        offColor = ren.material.color;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!CharacterMovementScript.characterMovementScript.isSitting || isFlashing)
            return;
        //Calculates hit speed
        Rigidbody collRb = collision.gameObject.GetComponent<Rigidbody>();
        float force = Mathf.Abs(collision.impulse.y);
        //print(force);
        if (collRb != null)
        {
            //float force = Mathf.Abs(collRb.velocity.y*collRb.mass - rb.velocity.y*rb.mass);
            force = Mathf.Abs(collRb.mass * collision.relativeVelocity.y)*0.75f;
            ButtonPress(force);
        }
        else if(rb.velocity.y <= 0)
        {
            //float force = -rb.velocity.y;
            ButtonPress(force);
        }
    }
    private void ButtonPress(float force)
    {
        print(force);
        force -= minForce;
        PlayerParticleManager.playerParticleManager.SetParticleBurstCount("LightbulbElectricity", (int)(force));
        PlayerParticleManager.playerParticleManager.PlayParticle("LightbulbElectricity");
        StartCoroutine(StartLight(force));
    }
    IEnumerator StartLight(float force)
    {
        isFlashing = true;
        ren.material.SetColor("_EmissionColor", onColor);

        Vector3 localPos = ren.transform.localPosition;
        localPos.y += 0.07f;
        ren.transform.localPosition = localPos;
        float ratio = Mathf.Min(1f, (force / (winForce-minForce)));

        Color c = lightbulbMatColor;

        for (int x = 0; x <= 30; x++)
        {
            float intensityValue = emissionMinIntensity + (1 - x / 30f) * ratio * (emissionmaxIntensity - emissionMinIntensity);

            c *= intensityValue;
            lightbulbMat.material.SetColor("_EmissionColor", c);
            c = lightbulbMatColor;
            bulbLight.intensity = minIntensity + (1 - x / 30f) * ratio * (maxIntensity - minIntensity);
            yield return new WaitForFixedUpdate();
        }
        c *= 0;
        lightbulbMat.material.SetColor("_EmissionColor", c);
        bulbLight.intensity = 0;
        isFlashing = false;
        ren.material.SetColor("_EmissionColor", offColor);
        localPos.y -= 0.07f;
        ren.transform.localPosition = localPos;
    }
}
