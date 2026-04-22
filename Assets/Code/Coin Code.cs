using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCode : MonoBehaviour
{
    public CoinTrackerScript playerCoinData;
    public int coinValue = 1;
    public float rotationSpeed = 50f;
    
    void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime); // Rotate the coin around the Y-axis for a spinning effect.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerCoinData.IncreaseCoinCount(coinValue);
            print("player is up in here");
            Destroy(gameObject);
        }
    }
}
