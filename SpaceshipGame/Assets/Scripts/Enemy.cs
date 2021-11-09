using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float shield;
    public ParticleSystem particleDestroyed;

    /*
public IEnumerator Shoot()
{
    yield return new WaitForSeconds(cooldown);

    for (int i = 0; i < pointShoot.Length; i++)
    {
        if (Physics.Raycast(pointShoot[i].position, pointShoot[i].TransformDirection(Vector3.forward), out hitAim, targeting))
        {
            if (hitAim.transform.tag == "Enemy" && targetEnemy != null)
            {
                var newProjectile = Instantiate(projectile, gunFront.transform.position, gunFront.transform.rotation) as GameObject;
                listProjectiles.Add(newProjectile);
                newProjectile.GetComponent<Projectile>().scriptShip = this;
                newProjectile.GetComponent<Projectile>().scriptEnemies = scriptEnemies;
                newProjectile.transform.parent = instances.transform;
                audioSource.PlayOneShot(clipShoot, 0.1f);
            }
        }
    }

    StartCoroutine(Shoot());
}

public IEnumerator TargetEnemy()
{
    yield return new WaitForSeconds(1.0f);

    for (int i = 0; i < scriptEnemies.listEnemy.Count; i++)
    {
        distanceEnemy = Vector3.Distance(ship.transform.position, scriptEnemies.listEnemy[i].transform.position);

        if (distanceEnemy > 0 && distanceEnemy < targeting)
        {
            if (distanceEnemy <= distanceEnemyShortest)
            {
                distanceEnemyShortest = distanceEnemy;
                targetEnemy = scriptEnemies.listEnemy[i];
            }
        }
    }

    StartCoroutine(TargetEnemy());
}
*/
}
