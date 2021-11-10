using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Proton : MonoBehaviour
{
    //Scripts
    public Enemies scriptEnemies;

    void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            scriptEnemies.ProtonDestroyed(this.gameObject);
        }
    }
}
