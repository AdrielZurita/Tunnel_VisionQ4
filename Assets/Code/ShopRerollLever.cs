using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopRerollLever : MonoBehaviour
{
    public int baseRerollCost = 4;
    private int rerollCost;
    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        rerollCost = baseRerollCost;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RerollShop()
    {   
        //if (ShopManager.Instance.coins < rerollCost) return;
        //ShopManager.Instance.RerollShop();
        //coins -= rerollCost;
        animator.SetTrigger("Pulled");
        
    }

}
