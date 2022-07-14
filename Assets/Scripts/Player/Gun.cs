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

    private RaycastHit hit;

    public bool shotCreature;

    // Start is called before the first frame update
    void Awake()
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
        
        if(Physics.Raycast(fpsCam.position, fpsCam.forward, out hit, range))
        {
            if(hit.transform.CompareTag("Creature"))
            {
                shotCreature = true;
            }
            Quaternion impactRotation = Quaternion.LookRotation(hit.normal);
            GameObject bulletImpactHole = Instantiate(bulletHole, hit.point, impactRotation);
            bulletImpactHole.transform.parent = hit.transform;
            Destroy(bulletImpactHole, 3);
            if(hit.transform.CompareTag("CreatureHead"))
            {
                return;
            }
            
        }
        
    }
}
