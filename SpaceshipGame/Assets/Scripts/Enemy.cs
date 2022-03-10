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
    private int countBarrel;
    public GameObject polygonBeamStatic;

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

    [Header("Drone")]
    public bool isStartShoot;
    public bool isInFlyPosition;
    public bool isFlyPointEntrance;
    public int flyGroupNumber;
    public int flyPositionNumber;

    [Header("Mothership")]
    public Transform[] explosionPoint;
    public ParticleSystem particleLightningExplosion;
    public ParticleSystem particleDomeExplosion;
    public ParticleSystem particleNovaExplosion;

    [Header("Audio")]
    private AudioSource audioSource;
    public AudioClip[] clipShootProton;
    public AudioClip[] clipShootPlasma;
    public AudioClip[] clipShootRocket;
    public AudioClip[] clipShootLaser;

    [Header("Animation")]
    public Animator animatorPunisher;

    [Header("UI")]
    public Image shieldBar;

    private void Start()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
        shield = startShield;
        reloadTime = Random.Range(reloadMinimum, reloadMaximum);

        if (enemyName == "Frigate" || enemyName == "Punisher")
        { 
            StartCoroutine(Shoot());
        }
        else if (enemyName == "Wing" || enemyName == "Drone" || enemyName == "SquidFrigate" || enemyName == "SquidProbe")
        {
            StartCoroutine(DelayShoot());
        }
        else if (enemyName == "SquidDestroyer")
        {
            StartCoroutine(ShootLaser());
        }
    }

    public IEnumerator ShootLaser()
    {
        if (!scriptEnemies.isEnemyDestroyed)
        {
            scriptEnemies.audioSource.PlayOneShot(scriptEnemies.clipMS_Target[Random.Range(0, scriptEnemies.clipMS_Target.Length)], 1.0f);
            yield return new WaitForSeconds(1.0f);
            scriptEnemies.handlingSquidDestroyer = 1.25f;
            scriptEnemies.thrustSquidDestroyer = 3.0f;
            scriptEnemies.audioSource.PlayOneShot(scriptEnemies.clipMS_Voice[Random.Range(0, scriptEnemies.clipMS_Voice.Length)], 1.0f);
            yield return new WaitForSeconds(2.0f);
            scriptEnemies.handlingSquidDestroyer = 0.1f;

            if (!scriptEnemies.isEnemyDestroyed)
            {
                polygonBeamStatic.SetActive(true);
                audioSource.PlayOneShot(clipShootLaser[0], 0.75f);
                audioSource.PlayOneShot(clipShootLaser[1], 0.75f);
                yield return new WaitForSeconds(3.0f);
                scriptEnemies.handlingSquidDestroyer = 0.25f;
                scriptEnemies.thrustSquidDestroyer = 20.0f;
            }

            polygonBeamStatic.SetActive(false);
            StartCoroutine(ShootLaser());
        }
    }

    public IEnumerator DelayShoot()
    {
        yield return new WaitForSeconds(3.0f);
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
        else if (enemyName == "Wing" || enemyName == "SquidFrigate")
        {
            for (int i = 0; i < barrel.Length; i++)
            {
                spawnedProjectile = Instantiate(scriptEnemies.prefabPlasma, barrel[i].transform.position, barrel[i].transform.rotation);
                scriptEnemies.listPlasma.Add(spawnedProjectile);
                spawnedProjectile.transform.parent = scriptEnemies.gameObject.transform;
                spawnedProjectile.GetComponent<Plasma>().scriptEnemies = scriptEnemies;
                audioSource.PlayOneShot(clipShootPlasma[Random.Range(0, clipShootPlasma.Length)], 0.25f);
            }

            StartCoroutine(Shoot());
        }
        else if (enemyName == "Drone" || enemyName == "SquidProbe")
        {
            for (int i = 0; i < barrel.Length; i++)
            {
                spawnedProjectile = Instantiate(scriptEnemies.prefabRocket, barrel[i].transform.position, barrel[i].transform.rotation);
                scriptEnemies.listRocket.Add(spawnedProjectile);
                spawnedProjectile.transform.parent = scriptEnemies.gameObject.transform;
                spawnedProjectile.GetComponent<Rocket>().scriptEnemies = scriptEnemies;
                audioSource.PlayOneShot(clipShootRocket[Random.Range(0, clipShootRocket.Length)], 0.25f);
                yield return new WaitForSeconds(1.5f);
            }

            StartCoroutine(Shoot());
        }
        else if (enemyName == "Punisher")
        {
            animatorPunisher.SetBool("Fire Attack Front", true);

            for (int i = 0; i < 48; i++)
            {
                spawnedProjectile = Instantiate(scriptEnemies.prefabRocket, barrel[countBarrel].transform.position, barrel[countBarrel].transform.rotation);
                scriptEnemies.listRocket.Add(spawnedProjectile);
                spawnedProjectile.transform.parent = scriptEnemies.gameObject.transform;
                spawnedProjectile.GetComponent<Rocket>().scriptEnemies = scriptEnemies;
                audioSource.PlayOneShot(clipShootRocket[Random.Range(0, clipShootRocket.Length)], 0.25f);
                countBarrel += 1;
                yield return new WaitForSeconds(0.1f);

                if (countBarrel >= barrel.Length)
                {
                    countBarrel = 0;
                }
            }

            animatorPunisher.SetBool("Fire Attack Front", false);
            StartCoroutine(Shoot());
        }
    }

    public void TakeDamage(float amount, string projectile)
    {
        shield -= amount;
        shieldBar.fillAmount = shield / startShield;

        if (shield <= 0f && enemyName != "SquidDestroyer" && enemyName != "Punisher")
        {
            var newParticle = Instantiate(particleDestroyed, transform.position, transform.rotation);
            newParticle.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
            newParticle.Play();
            scriptEnemies.listEnemy.Remove(gameObject);
            scriptEnemies.enemyDestroyed = gameObject;
            scriptEnemies.scriptShip.distanceEnemyShortest = 200f;
            scriptEnemies.StartCameraShake();

            if (projectile == "Projectile" && !isDead)
            {
                isDead = true;
                scriptEnemies.scriptGameSettings.UpdateScore();
                scriptEnemies.scriptGameSettings.UpdateEnemyKOValue();
            }

            scriptEnemies.EnemyDestroyed();
        }
        else if (shield <= 0f && enemyName == "Punisher")
        {
            if (!scriptEnemies.isEnemyDestroyed)
            {
                scriptEnemies.isEnemyDestroyed = true;
                StartCoroutine(DestroyPunisher());
            }
        }
        else if (shield <= 0f && enemyName == "SquidDestroyer")
        {
            if (!scriptEnemies.isEnemyDestroyed)
            {
                scriptEnemies.isEnemyDestroyed = true;
                scriptEnemies.countSquidDestroyerDestroyed += 1;
                StartCoroutine(DestroyMothership());
            }
        }
    }

    public IEnumerator DestroyPunisher()
    {
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < explosionPoint.Length; i++)
            {
                Instantiate(particleLightningExplosion, explosionPoint[i].position, transform.rotation);
                audioSource.PlayOneShot(scriptEnemies.clipLightningExplosion[Random.Range(0, scriptEnemies.clipLightningExplosion.Length)], 1.0f);
                scriptEnemies.handlingPunisher = 0.0f;
                scriptEnemies.thrustPunisher = 0.0f;
                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            }

            Instantiate(particleNovaExplosion, transform.position, transform.rotation);
            audioSource.PlayOneShot(scriptEnemies.clipNovaExplosion, 1.0f);
        }

        Instantiate(particleDomeExplosion, transform.position, transform.rotation);
        scriptEnemies.PlayClipDomeExplosion();
        var newParticle = Instantiate(particleDestroyed, transform.position, transform.rotation);
        newParticle.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
        newParticle.Play();
        scriptEnemies.listEnemy.Remove(gameObject);
        scriptEnemies.enemyDestroyed = gameObject;
        scriptEnemies.scriptShip.distanceEnemyShortest = 200f;
        scriptEnemies.StartCameraShake();
        scriptEnemies.scriptGameSettings.PlayMissionComplete();
        scriptEnemies.EnemyDestroyed();
    }

    public IEnumerator DestroyMothership()
    {
        scriptEnemies.audioSource.PlayOneShot(scriptEnemies.clipMS_Charge[0], 0.25f);
        scriptEnemies.audioSource.PlayOneShot(scriptEnemies.clipMS_Charge[1], 0.25f);

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < explosionPoint.Length; i++)
            {
                Instantiate(particleLightningExplosion, explosionPoint[i].position, transform.rotation);
                audioSource.PlayOneShot(scriptEnemies.clipLightningExplosion[Random.Range(0, scriptEnemies.clipLightningExplosion.Length)], 1.0f);
                scriptEnemies.handlingSquidDestroyer = 0.0f;
                scriptEnemies.thrustSquidDestroyer = 0.0f;
                polygonBeamStatic.SetActive(false);
                yield return new WaitForSeconds(Random.Range(0.05f, 0.1f));
            }

            Instantiate(particleNovaExplosion, transform.position, transform.rotation);
            audioSource.PlayOneShot(scriptEnemies.clipNovaExplosion, 1.0f);
        }

        Instantiate(particleDomeExplosion, transform.position, transform.rotation);
        scriptEnemies.PlayClipDomeExplosion();
        var newParticle = Instantiate(particleDestroyed, transform.position, transform.rotation);
        newParticle.gameObject.transform.parent = scriptEnemies.particlesObject.transform;
        newParticle.Play();
        scriptEnemies.listEnemy.Remove(gameObject);
        scriptEnemies.enemyDestroyed = gameObject;
        scriptEnemies.scriptShip.distanceEnemyShortest = 200f;
        scriptEnemies.StartCameraShake();

        if (scriptEnemies.countSquidDestroyerDestroyed >= 1)
        { 
            scriptEnemies.scriptGameSettings.PlayMissionComplete();
        }

        scriptEnemies.EnemyDestroyed();
    }
}
