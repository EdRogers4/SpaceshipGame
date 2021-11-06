using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ship scriptShip;
    public Enemies scriptEnemies;

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
            collision.gameObject.GetComponent<Enemy>().shield -= scriptShip.blasters;

            if (collision.gameObject.GetComponent<Enemy>().shield <= 0f)
            {
                collision.gameObject.GetComponent<Enemy>().particleDestroyed.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
                collision.gameObject.GetComponent<Enemy>().particleDestroyed.Play();
                scriptEnemies.DestroyParticle(collision.gameObject.GetComponent<Enemy>().particleDestroyed.gameObject);
                scriptEnemies.listEnemy.Remove(collision.gameObject);
                scriptEnemies.enemyDestroyed = collision.gameObject;
                scriptEnemies.EnemyDestroyed();
                scriptShip.distanceEnemyShortest = scriptShip.targeting;
            }

            DestroyProjectile();
        }
    }

    public void DestroyProjectile()
    {
        scriptShip.listProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }
}
