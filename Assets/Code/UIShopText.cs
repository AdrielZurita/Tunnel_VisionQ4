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

    void Update()
    {
        shopCoinCountText.text = ShopCoinData.playerCountCount.ToString() + " Shards";
        shopLivesText.text = shopPlayerLives.currentLives.ToString() + "/" + shopPlayerLives.maxLives.ToString() + " Lives";

    }
}
