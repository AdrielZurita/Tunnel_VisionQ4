using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    // coins
    public CoinTrackerScript playerCoinData;
    public TextMeshProUGUI coinCountText;

    // speed
    public PlayerMovementTunnelVision PlayerMovementTunnelVision;
    public TextMeshProUGUI speedText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        coinCountText.text = playerCoinData.playerCountCount.ToString() + " coins";
        speedText.text = Mathf.Round(PlayerMovementTunnelVision.rb.velocity.magnitude * 3.5f).ToString() + " MPH";
    }
}
