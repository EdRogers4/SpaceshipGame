using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proton : MonoBehaviour
{
    //Scripts
    public Enemies scriptEnemies;
    public float blasters;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            scriptEnemies.ProtonDestroyed(this.gameObject);
            scriptEnemies.scriptShip.TakeDamage(blasters);
        }
        else if (collision.transform.tag == "Enemy")
        {
            scriptEnemies.ProtonDestroyed(this.gameObject);
            collision.gameObject.GetComponent<Enemy>().TakeDamage(blasters);
        }
    }
}
