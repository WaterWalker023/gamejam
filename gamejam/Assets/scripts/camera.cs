using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour
{
    public float mouseS = 100f;
    public Transform player;

    float xRotation = 0f;
    public GameObject gun;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseS * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseS * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        player.Rotate(Vector3.up * mouseX);
        if (Input.GetMouseButtonDown(1))
        {
            Camera.main.fieldOfView = 30;
            gun.SetActive(false);
            mouseS -= 500;
        }
        if (Input.GetMouseButtonUp(1))
        {
            Camera.main.fieldOfView = 50;
            gun.SetActive(true);
            mouseS += 500;
        }
    }
}
