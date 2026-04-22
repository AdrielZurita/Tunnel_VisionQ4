using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCameramovement : MonoBehaviour
{
    public PlayerSettings sensetivityInfo;

    public Transform orientation;
    public Transform cameraPosition;

    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * sensetivityInfo.mouseSensitivityX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * sensetivityInfo.mouseSensitivityY;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(transform.localRotation.eulerAngles.x, yRotation, transform.localRotation.eulerAngles.z);
        orientation.localRotation = Quaternion.Euler(orientation.localRotation.eulerAngles.x, yRotation, 0);
        cameraPosition.localRotation = Quaternion.Euler(xRotation, yRotation, 0);
    }
}
