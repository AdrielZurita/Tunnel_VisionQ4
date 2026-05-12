using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRerollLever : MonoBehaviour
{
    public int baseRerollCost = 4;
    private int rerollCost;
    private Animator leverAnimator;
    private Animator slotAnimator;
    public CoinTrackerScript playerRerollCoins;


    // Start is called before the first frame update
    void Start()
    {
        rerollCost = baseRerollCost;
        leverAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RerollShop()
    {   
        if (playerRerollCoins.playerCountCount >= rerollCost)
        {
            playerRerollCoins.playerCountCount -= rerollCost;
            rerollCost++;
            leverAnimator.SetTrigger("Pulled");
            slotAnimator.SetTrigger("Reroll");
        }
        else
        {
            Debug.Log("Not enough shards to reroll the shop!");
            leverAnimator.SetTrigger("Poor");
        }
        
    }

}
