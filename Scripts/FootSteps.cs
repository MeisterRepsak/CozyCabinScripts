using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootSteps : MonoBehaviour
{
    AudioSource source;

    public AudioClip[] clips;
    void Start()
    {
        source = transform.parent.GetComponent<AudioSource>();
    }

    public void PlayFootstepA()
    {
        source.PlayOneShot(clips[0]);
    }
    public void PlayFootstepB()
    {
        source.PlayOneShot(clips[1]);
    }
}
