using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gun : MonoBehaviour
{
    public float damege = 10f;
    public float range = 100f;
    public float impactforce = 30f;
    public float firerate = 15f;

    public Camera fpscam;
    public ParticleSystem muzzleflash;
    public GameObject impacteff;

    private float nexttimetofire = 1f;
   
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Time.time >= nexttimetofire )
        {
            nexttimetofire = Time.time + 1f / firerate;
            shoot();
        }

       
    }
    void shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            muzzleflash.Play();

            RaycastHit hit;
            if (Physics.Raycast(fpscam.transform.position, fpscam.transform.forward, out hit, range))
            {
                Debug.Log(hit.transform.name);
                targe target = hit.transform.GetComponent<targe>();
                if (target != null)
                {
                    target.takedamage(damege);
                }

                if (hit.rigidbody != null)
                {
                    hit.rigidbody.AddForce(-hit.normal * impactforce);
                }

                GameObject impactGO = Instantiate(impacteff, hit.point, Quaternion.LookRotation(hit.normal));
                Destroy(impactGO, 2f);
            }
        }
        

    }
}
