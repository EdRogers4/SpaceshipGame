using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool isWeapon;
    public bool isShield;
    public Ship scriptShip;
    public ParticleSystem particlePickupWeapon;
    public ParticleSystem particlePickupShield;
    private ParticleSystem newPrefabParticle;

    public void WeaponUpgrade()
    {
        newPrefabParticle = Instantiate(particlePickupWeapon, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }

    public void ShieldUpgrade()
    {
        newPrefabParticle = Instantiate(particlePickupShield, transform.position, transform.rotation);
        gameObject.SetActive(false);
    }
}
