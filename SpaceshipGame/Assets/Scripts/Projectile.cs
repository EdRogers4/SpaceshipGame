using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public Ship scriptShip;

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
            DestroyProjectile();
        }
        else
        {
            Debug.Log("Projectile Hit: " + collision.transform.name);
        }
    }

    public void DestroyProjectile()
    {
        scriptShip.listProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }
}
