using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ship scriptShip;
    public Enemies scriptEnemies;
    public ParticleSystem particleExplode;

    private ParticleSystem newParticleExplode;

    void Start()
    {
        StartCoroutine(DelayDestroy());
    }

    public IEnumerator DelayDestroy()
    {
        yield return new WaitForSeconds(2.0f);
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

            DestroyProjectile();
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