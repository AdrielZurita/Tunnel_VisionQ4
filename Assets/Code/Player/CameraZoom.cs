using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public float zoomosity = 20f;
    public PlayerMovementTunnelVision PlayerMovementTunnelVision;

    // Update is called once per frame
    void Update()
    { 
        Camera.main.fieldOfView = PlayerMovementTunnelVision.currentZoom * zoomosity + 60f;
    }
}
