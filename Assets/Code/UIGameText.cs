using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIGameText : MonoBehaviour
{
    // coins
    public CoinTrackerScript playerCoinData;
    public TextMeshProUGUI coinCountText;

    // speed
    public PlayerMovementTunnelVision PlayerMovementTunnelVision;
    public TextMeshProUGUI speedText;

    //lives
    public PlayerSettings playerLives;
    public TextMeshProUGUI livesText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinCountText.text = playerCoinData.playerCountCount.ToString() + " Shards";
        speedText.text = Mathf.Round(PlayerMovementTunnelVision.rb.velocity.magnitude * 3.5f).ToString() + " MPH";
        livesText.text = playerLives.currentLives.ToString() + " / " + playerLives.maxLives.ToString() + " Lives";
    }
}
