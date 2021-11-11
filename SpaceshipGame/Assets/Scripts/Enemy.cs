using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("World")]
    public ParticleSystem particleDestroyed;
    private GameObject spawnedProjectile;
    public GameObject gun;

    [Header("Scripts")]
    public Enemies scriptEnemies;

    [Header("Stats")]
    public float shield;
    public float startShield;
    public float reloadTime;
    public float reloadMinimum;
    public float reloadMaximum;

    [Header("UI")]
    public Image shieldBar;

    private void Start()
    {
        shield = startShield;
        reloadTime = Random.Range(reloadMinimum, reloadMaximum);
        StartCoroutine(Shoot());
    }
    
    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(reloadTime);
        spawnedProjectile = Instantiate(scriptEnemies.prefabProton, gun.transform.position, gun.transform.rotation);
        scriptEnemies.listProton.Add(spawnedProjectile);
        spawnedProjectile.transform.parent = scriptEnemies.gameObject.transform;
        spawnedProjectile.GetComponent<Proton>().scriptEnemies = scriptEnemies;
    }

    public void TakeDamage(float amount)
    {
        shield -= amount;
        shieldBar.fillAmount = shield / startShield;

        if (shield <= 0f)
        {
            particleDestroyed.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
            particleDestroyed.Play();
            scriptEnemies.DestroyParticle(particleDestroyed.gameObject);
            scriptEnemies.listEnemy.Remove(this.gameObject);
            scriptEnemies.enemyDestroyed = this.gameObject;
            scriptEnemies.EnemyDestroyed();
            scriptEnemies.scriptShip.distanceEnemyShortest = 200f;
        }
    }

    /*
 *
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
