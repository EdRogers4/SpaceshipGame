using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    public Camera cameraMain;
    public Enemy scriptEnemy;

    private void Start()
    {
        if (cameraMain == null && scriptEnemy.scriptEnemies.cameraMain != null)
        {
            cameraMain = scriptEnemy.scriptEnemies.cameraMain;
        }
    }

    void Update()
    {
        if (cameraMain != null)
        {
            transform.rotation = cameraMain.transform.rotation;
        }
    }
}
