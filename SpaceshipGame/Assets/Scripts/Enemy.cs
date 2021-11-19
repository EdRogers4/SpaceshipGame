using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("World")]
    public ParticleSystem particleDestroyed;
    private GameObject spawnedProjectile;
    public GameObject[] barrel;

    [Header("Scripts")]
    public Enemies scriptEnemies;

    [Header("Stats")]
    public string enemyName;
    public bool isDead;
    public float shield;
    public float startShield;
    public float reloadTime;
    public float reloadMinimum;
    public float reloadMaximum;

    [Header("Stats")]
    private AudioSource audioSource;
    public AudioClip[] clipShootProton;
    public AudioClip[] clipShootPlasma;

    [Header("UI")]
    public Image shieldBar;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        shield = startShield;
        reloadTime = Random.Range(reloadMinimum, reloadMaximum);
        StartCoroutine(Shoot());
    }
    
    public IEnumerator Shoot()
    {
        yield return new WaitForSeconds(reloadTime);

        if (enemyName == "Frigate")
        {
            spawnedProjectile = Instantiate(scriptEnemies.prefabProton, barrel[0].transform.position, barrel[0].transform.rotation);
            scriptEnemies.listProton.Add(spawnedProjectile);
            spawnedProjectile.transform.parent = scriptEnemies.gameObject.transform;
            spawnedProjectile.GetComponent<Proton>().scriptEnemies = scriptEnemies;
            audioSource.PlayOneShot(clipShootProton[Random.Range(0, clipShootProton.Length)], 0.55f);
        }
        else if (enemyName == "Wing")
        {
            for (int i = 0; i < barrel.Length; i++)
            {
                spawnedProjectile = Instantiate(scriptEnemies.prefabPlasma, barrel[i].transform.position, barrel[i].transform.rotation);
                scriptEnemies.listPlasma.Add(spawnedProjectile);
                spawnedProjectile.transform.parent = scriptEnemies.gameObject.transform;
                spawnedProjectile.GetComponent<Plasma>().scriptEnemies = scriptEnemies;
                audioSource.PlayOneShot(clipShootPlasma[Random.Range(0, clipShootPlasma.Length)], 0.55f);
            }

            StartCoroutine(Shoot());
        }
    }

    public void TakeDamage(float amount, string projectile)
    {
        shield -= amount;
        shieldBar.fillAmount = shield / startShield;

        if (shield <= 0f)
        {
            var newParticle = Instantiate(particleDestroyed, transform.position, transform.rotation);
            newParticle.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
            newParticle.Play();
            scriptEnemies.listEnemy.Remove(gameObject);
            scriptEnemies.enemyDestroyed = gameObject;
            scriptEnemies.scriptShip.distanceEnemyShortest = 200f;

            if (projectile == "Projectile" && !isDead)
            {
                isDead = true;
                scriptEnemies.scriptGameSettings.UpdateScore();
                scriptEnemies.scriptGameSettings.UpdateEnemyKOValue();
            }

            scriptEnemies.EnemyDestroyed();
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
