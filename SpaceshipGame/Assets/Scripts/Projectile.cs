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
                scriptEnemies.listEnemy.Remove(collision.gameObject);
                Destroy(collision.gameObject);
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
