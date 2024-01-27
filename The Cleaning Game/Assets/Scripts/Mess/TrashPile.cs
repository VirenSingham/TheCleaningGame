using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashPile : Mess
{
    [SerializeField] List<AudioSource> TrashNoises;
    [SerializeField] float TimeBetweenNoises1;
    [SerializeField] float TimeBetweenNoises2;

    float time;
    int numbOfNoises;

    private void Update()
    {
        time -= Time.deltaTime;
        if (time < 0)
        {
            PlayRandomNoise();
            time = GenerateNewTiming();
        }
    }

    private float GenerateNewTiming()
    {
        return UnityEngine.Random.Range(TimeBetweenNoises1, TimeBetweenNoises2);
    }

    private void PlayRandomNoise()
    {
        int r = UnityEngine.Random.Range(0, numbOfNoises);
        TrashNoises[r].Play();
    }

    private void Awake()
    {
        time = GenerateNewTiming();
        numbOfNoises = TrashNoises.Count;
    }
}
