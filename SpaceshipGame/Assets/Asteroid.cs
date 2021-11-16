using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Asteroid : MonoBehaviour
{
    [Header("World")]
    public Enemies scriptEnemies;

    [Header("World")]
    public Transform destination;

    [Header("Stats")]
    public float shield;
    public float size;
    public float sizeMinimum;
    public float sizeMaximum;

    [Header("Particles")]
    public ParticleSystem particleRockSlide;
    public ParticleSystem particleFallingRocks;
    private ParticleSystem newParticleRockSlide;
    private ParticleSystem newParticleFallingRocks;

    [Header("Distance")]
    public float distanceDestination;

    private void Start()
    {
        StartCoroutine(MeasureDistanceToDestination());
        size = Random.Range(sizeMinimum, sizeMaximum);
        transform.localScale = new Vector3(size, size, size);
        shield = size;
    }

    public IEnumerator MeasureDistanceToDestination()
    {
        distanceDestination = Vector3.Distance(transform.position, destination.position);

        if (distanceDestination < 0.1f)
        {
            scriptEnemies.listAsteroid.Remove(gameObject);
            Destroy(gameObject);
        }

        yield return new WaitForSeconds(2.0f);
        StartCoroutine(MeasureDistanceToDestination());
    }

    public void TakeDamage(float amount)
    {
        shield -= amount;

        if (shield <= 0f)
        {
            scriptEnemies.AsteroidDestroyed();
            newParticleFallingRocks = Instantiate(particleFallingRocks, gameObject.transform.position, gameObject.transform.rotation);
            newParticleRockSlide = Instantiate(particleRockSlide, gameObject.transform.position, gameObject.transform.rotation);
            newParticleFallingRocks.transform.localScale = new Vector3(size * 1.4f, size * 1.4f, size * 1.4f);
            newParticleRockSlide.transform.localScale = new Vector3(size * 1.4f, size * 1.4f, size * 1.4f);
            newParticleFallingRocks.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
            newParticleRockSlide.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
            newParticleFallingRocks.Play();
            newParticleRockSlide.Play();
            scriptEnemies.listAsteroid.Remove(gameObject);
            scriptEnemies.scriptGameSettings.UpdateScore();
            Destroy(gameObject);
        }
    }
}
