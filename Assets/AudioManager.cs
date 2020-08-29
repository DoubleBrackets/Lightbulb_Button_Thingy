using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{

    public static AudioManager audioManager;

    private void Awake()
    {
        audioManager = this;
    }

    public event Action<String> PlayAudioEvent;
    public void PlayAudio(string _id)
    {
        PlayAudioEvent?.Invoke(_id);
    }

    public event Action<String> StopAudioEvent;
    public void StopAudio(string _id)
    {
        StopAudioEvent?.Invoke(_id);
    }



}
