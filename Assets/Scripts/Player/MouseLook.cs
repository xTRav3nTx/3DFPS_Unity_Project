using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    [SerializeField] public float mouseSensitivity = 500f;

    public Transform playerBody;

    private float xRotation = 0;
    private float yRotation = 0;

    float lookY;
    float lookX;

    

    // Start is called before the first frame update
    void Start()
    {
        
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;
        lookX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        lookY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= lookY;
        yRotation += lookX;

        playerBody.Rotate(Vector3.up * lookX);

        xRotation = Mathf.Clamp(xRotation, -60f, 80f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.rotation = Quaternion.Euler(0, yRotation, 0);

    }
}
