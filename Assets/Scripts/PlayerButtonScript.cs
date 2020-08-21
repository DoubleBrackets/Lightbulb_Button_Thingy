using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerButtonScript : MonoBehaviour
{
    public static PlayerButtonScript playerButtonScript;

    public MeshRenderer ren;

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
        playerButtonScript = this;
        rb = gameObject.GetComponentInParent<Rigidbody>();
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
        PlayerParticleManager.playerParticleManager.SetParticleBurstCount("LightbulbElectricity", (int)(force));
        PlayerParticleManager.playerParticleManager.PlayParticle("LightbulbElectricity");
        StartCoroutine(StartLight(force));
    }

    IEnumerator StartLight(float force)
    {
        ren.material.SetColor("_EmissionColor", onColor);
        Vector3 localPos = ren.transform.localPosition;
        localPos.y += 0.07f;
        ren.transform.localPosition = localPos;
        isFlashing = true;
        float ratio = Mathf.Min(1f, (force / winForce));
        for(int x = 0;x <= 30;x++)
        {
            bulbLight.intensity = minIntensity + (1 - x / 30f) * ratio * (maxIntensity-minIntensity);
            yield return new WaitForFixedUpdate();
        }
        bulbLight.intensity = 0;
        isFlashing = false;
        ren.material.SetColor("_EmissionColor", offColor);
        localPos.y -= 0.07f;
        ren.transform.localPosition = localPos;
    }
}
