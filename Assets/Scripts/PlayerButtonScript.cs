﻿using System.Collections;
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

    private float minForce = 2f;
    private float winForce = 50f;

    private float minIntensity = -0.35f;
    private float maxIntensity = 8f;

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
            force = Mathf.Abs(Mathf.Max(rb.mass,collRb.mass) * collision.relativeVelocity.y)*0.75f;
            ButtonPress(force);
        }
        else if(rb.velocity.y <= 0)
        {
            //float force = -rb.velocity.y;
            ButtonPress(force*1.2f);
        }
    }
    private void ButtonPress(float force)
    {
        print(force);
        force -= minForce;
        UIScript.uiScript.SetChargeBar(force, (winForce - minForce));
        PlayerParticleManager.playerParticleManager.SetParticleBurstCount("LightbulbElectricity", (int)(force*0.6f));
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

        for (int x = 0; x <= 40; x++)
        {
            float intensityValue = emissionMinIntensity + (1 - x / 40f) * ratio * (emissionmaxIntensity - emissionMinIntensity);

            c *= intensityValue;
            lightbulbMat.material.SetColor("_EmissionColor", c);
            c = lightbulbMatColor;
            bulbLight.intensity = minIntensity + (1 - x / 40f) * ratio * (maxIntensity - minIntensity);
            yield return new WaitForFixedUpdate();
        }
        c *= 0;
        lightbulbMat.material.SetColor("_EmissionColor", c);
        bulbLight.intensity = 0;
        isFlashing = false;
        ren.material.SetColor("_EmissionColor", offColor);
        localPos.y -= 0.07f;
        ren.transform.localPosition = localPos;

        PlayerParticleManager.playerParticleManager.StopParticle("LightbulbElectricity");
    }
}
