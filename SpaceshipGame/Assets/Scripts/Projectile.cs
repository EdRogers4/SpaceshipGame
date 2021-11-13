using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ship scriptShip;
    public Enemies scriptEnemies;
    public bool isExploding;
    public ParticleSystem particleExplode;
    public GameObject prefabRocketExplosion;
    public GameObject prefabNukeExplosion;

    private ParticleSystem newParticleExplode;
    private GameObject newPrefabExplosive;

    void Start()
    {
        StartCoroutine(DelayDestroy());
    }

    public IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(5.0f);
        DestroyProjectile();
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

            if (scriptShip.shipName == "Bomber" && !isExploding)
            {
                newPrefabExplosive = Instantiate(prefabRocketExplosion, collision.contacts[0].point, transform.rotation);
                newPrefabExplosive.GetComponent<Projectile>().scriptShip = scriptShip;
                newPrefabExplosive.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newPrefabExplosive.GetComponent<Projectile>().isExploding = true;
                newPrefabExplosive.transform.parent = scriptShip.instances.transform;
            }
            else if (scriptShip.shipName == "Breaker" && !isExploding)
            {
                newPrefabExplosive = Instantiate(prefabNukeExplosion, collision.contacts[0].point, transform.rotation);
                newPrefabExplosive.GetComponent<Projectile>().scriptShip = scriptShip;
                newPrefabExplosive.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newPrefabExplosive.GetComponent<Projectile>().isExploding = true;
                newPrefabExplosive.transform.parent = scriptShip.instances.transform;
            }

            if (scriptShip.shipName != "Vanguard" && !isExploding)
            {
                DestroyProjectile();
            }
        }
        else if (collision.transform.tag == "Proton")
        {
            scriptEnemies.ProtonDestroyed(collision.gameObject);
        }
    }

    public void DestroyProjectile()
    {
        scriptShip.listProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }
}