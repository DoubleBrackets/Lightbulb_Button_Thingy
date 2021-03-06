﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleManager : MonoBehaviour
{
    public static PlayerParticleManager playerParticleManager;

    private void Awake()
    {
        playerParticleManager = this;
    }


    //Event system for player particles
    public event Action<String> playParticleEvent;
    public void PlayParticle(string _id)
    {
        playParticleEvent?.Invoke(_id);
    }

    public event Action<String> stopParticleEvent;
    public void StopParticle(string _id)
    {
        stopParticleEvent?.Invoke(_id);
    }

    public event Action<String,bool> setParticleActiveEvent;
    public void SetParticleActive(string _id, bool val)
    {
        if(setParticleActiveEvent != null)
        {
            setParticleActiveEvent(_id, val);
        }
    }
    public event Action<String, int> setParticleBurstCountEvent;
    public void SetParticleBurstCount(string _id, int val)
    {
        if (setParticleBurstCountEvent != null)
        {
            setParticleBurstCountEvent(_id, val);
        }
    }

    public event Action<String, int> setParticleEmissionEvent;
    public void SetParticleEmission(string _id, int val)
    {
        if (setParticleEmissionEvent != null)
        {
            setParticleEmissionEvent(_id, val);
        }
    }

    public event Action<String, float> setParticleDragEvent;
    public void SetParticleDrag(string _id, float val)
    {
        if (setParticleDragEvent != null)
        {
            setParticleDragEvent(_id, val);
        }
    }

}
