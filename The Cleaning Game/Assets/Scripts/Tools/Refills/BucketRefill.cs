using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BucketRefill : MonoBehaviour, Activatable
{
    [SerializeField] ParticleSystem particleEffect;

    public void Activate()
    {
        TurnOnWaterParticles();
    }

    private void TurnOnWaterParticles()
    {
        if (!particleEffect.isPlaying)
            particleEffect.Play();
    }
}
