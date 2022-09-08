using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class zoom : MonoBehaviour
{
    public GameObject gun;
    
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView = 30;
            gun.SetActive(false);
        }
        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = 50;
            gun.SetActive(true);
        }
    }
}
