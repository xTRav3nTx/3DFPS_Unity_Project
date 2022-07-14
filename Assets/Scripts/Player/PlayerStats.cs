using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    public float playerMaxHealth = 100f;
    public float currentPlayerHealth;

    public CreatureMovement creature;
    public PlayerMovement player;
    public UI ui;
    
        
    
        // Start is called before the first frame update
    void Awake()
    {
        currentPlayerHealth = playerMaxHealth;
        ui.SetMaxHealth(currentPlayerHealth);
    }

    // Update is called once per frame
    void Update()
    {
        if(player.wasHit)
        {
            currentPlayerHealth -= creature.attackPower;
            ui.Sethealth(currentPlayerHealth);
            player.wasHit = false;
        }
    }
}
