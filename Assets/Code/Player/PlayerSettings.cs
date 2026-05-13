using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
    public float mouseSensitivityY = 1000f;
    public float mouseSensitivityX = 1000f;
    public float currentLives = 3f;
    public float maxLives = 3f;
    public bool devmode = false;
    public int levelPartCount = 10;
}
