using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rocket : MonoBehaviour
{
    //Scripts
    public Enemies scriptEnemies;
    public float blasters;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            scriptEnemies.scriptShip.TakeDamage(blasters);
            scriptEnemies.RocketDestroyed(this.gameObject);
        }
        else if (collision.transform.tag == "Enemy")
        {
            collision.gameObject.GetComponent<Enemy>().TakeDamage(blasters, "Rocket");
            scriptEnemies.RocketDestroyed(this.gameObject);
        }
        else if (collision.transform.tag == "Asteroid")
        {
            collision.gameObject.GetComponent<Asteroid>().TakeDamage(blasters, "Rocket");
            scriptEnemies.RocketDestroyed(this.gameObject);
        }
    }
}
