using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureRayCast : MonoBehaviour
{
    private RaycastHit playerHit;
    [SerializeField] private Transform raycastHolder;
    [SerializeField] private LayerMask player;
    public bool lookingAtPlayer;

    // Update is called once per frame
    void Update()
    {
        lookingAtPlayer = Physics.Raycast(raycastHolder.position, raycastHolder.forward, out playerHit, player);  
    }

    
}
