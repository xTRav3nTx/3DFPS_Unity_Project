using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] private Transform player;
    private Vector3 playerPosition;

    [SerializeField] private Transform turnPivot;
    /*[SerializeField] private Transform targetVector;*/
    [SerializeField] private Transform turretbase;
    private float turnPivotAngle;
    private Vector3 currentAxis;

    private float distanceFromPlayer;

    private void Awake()
    {
        turnPivot.transform.RotateAround(turretbase.transform.position, -turnPivot.up, 20 * Time.deltaTime);
        currentAxis = -turnPivot.up;

    }

    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = Vector3.Distance(turnPivot.position, player.position);
        if(distanceFromPlayer > 30)
        {
            turretIdle();
        }
        else
        {
            turretAttack();
        }
        
    }

    void turretIdle()
    {
        turnPivotAngle = turnPivot.localEulerAngles.y;
        if (turnPivotAngle >= 150 && turnPivotAngle < 151)
        {
            turnPivot.transform.RotateAround(turretbase.transform.position, -turnPivot.up, 20 * Time.deltaTime);
            currentAxis = -turnPivot.up;
        }
        else if (turnPivotAngle <= 30 && turnPivotAngle > 29)
        {
            turnPivot.transform.RotateAround(turretbase.transform.position, turnPivot.up, 20 * Time.deltaTime);
            currentAxis = turnPivot.up;
        }
        else
        {
            turnPivot.transform.RotateAround(turretbase.transform.position, currentAxis, 20 * Time.deltaTime);
        }
    }

    void turretAttack() 
    {
        playerPosition = new Vector3(player.position.x, turnPivot.position.y, player.position.z);
        /*targetVector.transform.LookAt(playerPosition);*/
    }
}
