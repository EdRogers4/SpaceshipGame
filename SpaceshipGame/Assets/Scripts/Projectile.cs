using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ship scriptShip;
    public Enemies scriptEnemies;
    public Asteroid scriptAsteroid;
    public bool isExploding;
    public ParticleSystem particleExplode;
    public GameObject prefabNukeExplosion;

    private float distanceEnemy;
    private float distanceProton;
    private float distanceEnemyShortest;
    private GameObject targetEnemy;

    private ParticleSystem newParticleExplode;
    private GameObject newPrefabExplosive;

    void Start()
    {
        distanceEnemyShortest = 1000f;
        StartCoroutine(DelayDestroy());

        if (scriptShip.shipName == "Breaker")
        {
            StartCoroutine(TargetProton());
            StartCoroutine(TargetEnemy());
        }
    }

    private void FixedUpdate()
    {
        if (isExploding || scriptShip.shipName == "Fighter" || scriptShip.shipName == "Interceptor")
        {
            return;
        }
        else if (targetEnemy != null && scriptShip.shipName == "Breaker")
        {
            Vector3 targetDirection = targetEnemy.transform.position - transform.position;
            float singleStep = scriptShip.targeting * Time.deltaTime;
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);
            transform.rotation = Quaternion.LookRotation(newDirection);
        }
    }

    public IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(5.0f);
        DestroyProjectile();
    }

    public IEnumerator TargetProton()
    {
        for (int i = 0; i < scriptEnemies.listProton.Count; i++)
        {
            distanceProton = Vector3.Distance(transform.position, scriptEnemies.listProton[i].transform.position);

            if (distanceProton > 0 && distanceProton < 1000f)
            {
                if (distanceProton < distanceEnemyShortest)
                {
                    distanceEnemyShortest = distanceProton;
                    targetEnemy = scriptEnemies.listProton[i];
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        StartCoroutine(TargetProton());
    }

    public IEnumerator TargetEnemy()
    {
        for (int i = 0; i < scriptEnemies.listEnemy.Count; i++)
        {
            distanceEnemy = Vector3.Distance(transform.position, scriptEnemies.listEnemy[i].transform.position);

            if (distanceEnemy > 0 && distanceEnemy < 1000f)
            {
                if (distanceEnemy < distanceEnemyShortest)
                {
                    distanceEnemyShortest = distanceEnemy;
                    targetEnemy = scriptEnemies.listEnemy[i];
                }
            }
        }

        yield return new WaitForSeconds(0.25f);
        StartCoroutine(TargetEnemy());
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(scriptShip.blasters);

            foreach (ContactPoint contact in collision.contacts)
            {
                newParticleExplode = Instantiate(particleExplode, collision.contacts[0].point, transform.rotation);
                newParticleExplode.transform.parent = scriptEnemies.particlesObject.transform;
            }

            if (scriptShip.shipName == "Breaker" && !isExploding)
            {
                newPrefabExplosive = Instantiate(prefabNukeExplosion, collision.contacts[0].point, transform.rotation);
                newPrefabExplosive.GetComponent<Projectile>().scriptShip = scriptShip;
                newPrefabExplosive.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newPrefabExplosive.GetComponent<Projectile>().isExploding = true;
                newPrefabExplosive.transform.parent = scriptShip.instances.transform;
            }

            DestroyProjectile();
        }
        else if (collision.transform.tag == "Proton")
        {
            scriptEnemies.scriptGameSettings.UpdateScore();
            scriptEnemies.ProtonDestroyed(collision.gameObject);
        }
        else if (collision.transform.tag == "Asteroid")
        {
            foreach (ContactPoint contact in collision.contacts)
            {
                newParticleExplode = Instantiate(particleExplode, collision.contacts[0].point, transform.rotation);
                newParticleExplode.transform.parent = scriptEnemies.particlesObject.transform;
            }

            if (scriptShip.shipName == "Breaker" && !isExploding)
            {
                newPrefabExplosive = Instantiate(prefabNukeExplosion, collision.contacts[0].point, transform.rotation);
                newPrefabExplosive.GetComponent<Projectile>().scriptShip = scriptShip;
                newPrefabExplosive.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newPrefabExplosive.GetComponent<Projectile>().isExploding = true;
                newPrefabExplosive.transform.parent = scriptShip.instances.transform;
            }

            scriptAsteroid = collision.gameObject.GetComponent<Asteroid>();
            scriptAsteroid.TakeDamage(scriptShip.blasters);
            DestroyProjectile();
        }
    }

    public void DestroyProjectile()
    {
        scriptShip.listProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }
}