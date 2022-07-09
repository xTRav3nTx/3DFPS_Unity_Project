using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    bool isShooting;
    [SerializeField] private Transform fpsCam;
    [SerializeField] private float range = 100f;
    [SerializeField] private float impactForce = 150f;
    [SerializeField] private int fireRate = 10;
    private float nextTimetoFire = 0f;

    [SerializeField] private GameObject bulletHole;
    [SerializeField] private AudioSource shotSound;

    RaycastHit hit;

    // Start is called before the first frame update
    void Start()
    {
        shotSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        shoot();
    }

    void shoot()
    {
        if (Input.GetMouseButtonDown(0))
        {
            isShooting = true;
            if(isShooting && Time.time >= nextTimetoFire)
            {
                nextTimetoFire = Time.time + 1f / fireRate;
                Fire();
            }
            
        }
    }

    void Fire()
    {
        shotSound.Play();
        
        if (Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, range))
        {
            if(hit.rigidbody != null)
            {
                hit.rigidbody.AddForce(-hit.normal * impactForce);
            }
            
            

            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject bulletImpactHole = Instantiate(bulletHole, hit.point, impactRotation);
            bulletImpactHole.transform.parent = hit.transform;
            Destroy(bulletImpactHole, 3);
        }
    }
}
