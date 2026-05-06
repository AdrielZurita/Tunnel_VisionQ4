using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSettings", menuName = "ScriptableObjects/PlayerSettings", order = 1)]
public class PlayerSettings : ScriptableObject
{
 public float mouseSensitivityY = 1000f;
 public float mouseSensitivityX = 1000f;
}
