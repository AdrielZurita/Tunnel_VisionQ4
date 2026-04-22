using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    public PlayerMovementTunnelVision PlayerMovementTunnelVision;

    // Update is called once per frame
    void Update()
    { 
        Camera.main.fieldOfView = PlayerMovementTunnelVision.currentZoom * 20f + 60f; // Adjust the field of view based on the current zoom level. Ref. 1

    }
}
