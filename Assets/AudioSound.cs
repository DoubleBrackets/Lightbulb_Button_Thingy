using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSound : MonoBehaviour
{
    AudioSource source;

    public string id;

    public float startTime;
    // Start is called before the first frame update
    void Start()
    {
        source = gameObject.GetComponent<AudioSource>();
        AudioManager.audioManager.PlayAudioEvent += Play;
        AudioManager.audioManager.StopAudioEvent += Stop;
    }


    public void Play(string _id)
    {
        if(_id.CompareTo(id) == 0)
        {
            source.time = startTime;
            source.Play();
        }
    }
    public void Stop(string _id)
    {
        if (_id.CompareTo(id) == 0)
        {
            source.Stop();
        }
    }

}
