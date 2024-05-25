using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleEffect : Effect
{
    protected ParticleSystem particle;

    protected virtual void Awake()
    {
        particle = GetComponent<ParticleSystem>();
    }

    public void Play()
    {
        particle.Play();
        PlaySoundEffect();
    }
}
