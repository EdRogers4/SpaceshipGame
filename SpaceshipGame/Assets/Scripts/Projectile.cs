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
        scriptShip.listProjectiles.Remove(gameObject);
        Destroy(gameObject);
    }
}
