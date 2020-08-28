using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParticleScript : MonoBehaviour
{
    // Start is called before the first frame update
    public string particleId;

    ParticleSystem pSys;

    void Start()
    {
        pSys = gameObject.GetComponent<ParticleSystem>();
        PlayerParticleManager.playerParticleManager.setParticleActiveEvent += SetActive;
        PlayerParticleManager.playerParticleManager.playParticleEvent += Play;
        PlayerParticleManager.playerParticleManager.stopParticleEvent += Stop;
        PlayerParticleManager.playerParticleManager.setParticleBurstCountEvent += SetBurstCount;
        PlayerParticleManager.playerParticleManager.setParticleEmissionEvent += SetEmission;
        PlayerParticleManager.playerParticleManager.setParticleDragEvent += SetDrag;
    }


    void SetActive(string _id, bool val)
    {
        if(_id.CompareTo(particleId) == 0)
        {
            if(val)
            {
                var c = pSys.main;
                c.maxParticles = 1000;
            }
            else
            {
                var c = pSys.main;
                c.maxParticles = 0;
            }
        }
    }

    void Play(string _id)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            if(!pSys.isPlaying)
                pSys.Play();
        }
    }

    void Stop(string _id)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            if (pSys.isPlaying)
                pSys.Stop();
        }
    }

    void SetBurstCount(string _id, int val)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            var em = pSys.emission;
            ParticleSystem.Burst burst = em.GetBurst(0);
            burst.count = val;
            em.SetBurst(0,burst);
        }
    }

    void SetEmission(string _id, int val)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            var em = pSys.emission;
            em.rateOverTime = val;
        }
    }
    void SetDrag(string _id, float val)
    {
        if (_id.CompareTo(particleId) == 0)
        {
            var lim = pSys.limitVelocityOverLifetime;
            lim.drag = val;
        }
    }
}
