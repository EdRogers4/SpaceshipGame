using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTimer : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DestroyObject());
    }

    public IEnumerator DestroyObject()
    {
        yield return new WaitForSeconds(5.0f);
        Destroy(gameObject);
    }
}
