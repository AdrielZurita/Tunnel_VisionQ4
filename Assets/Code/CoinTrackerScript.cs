using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CoinTracker", menuName = "ScriptableObjects/CoinTracker", order = 1)]
public class CoinTrackerScript : ScriptableObject
{
    public float playerCountCount = 0;

    public void IncreaseCoinCount(int coinValue)
    {
        playerCountCount += coinValue;
    }
}
