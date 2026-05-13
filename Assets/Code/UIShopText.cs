using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIShopText : MonoBehaviour
{
    // coins
    public CoinTrackerScript ShopCoinData;
    public TextMeshProUGUI shopCoinCountText;
    //lives
    public PlayerSettings shopPlayerLives;
    public TextMeshProUGUI shopLivesText;

    public PlayerSettings devModeCash;
 

    void Update()
    {
        shopCoinCountText.text = ShopCoinData.playerCountCount.ToString() + " Shards";
        shopLivesText.text = shopPlayerLives.currentLives.ToString() + "/" + shopPlayerLives.maxLives.ToString() + " Lives";

        if (devModeCash.devmode)
        {
            if (Input.GetKeyDown(KeyCode.F2))
            {
                ShopCoinData.IncreaseCoinCount(1);
            }
        }
    }
}
