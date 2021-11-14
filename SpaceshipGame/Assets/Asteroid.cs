using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [Header("World")]
    public Transform destination;

    [Header("Stats")]
    public float shield;
    public float speed;

    [Header("Particles")]
    public ParticleSystem particleRockSlide;
    public ParticleSystem particleFallingRocks;
}
