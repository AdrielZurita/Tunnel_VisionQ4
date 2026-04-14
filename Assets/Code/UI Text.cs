using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIText : MonoBehaviour
{
    public TextMeshProUGUI speedText;
    public PlayerMovementV3 PlayerMovementV3;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speedText.text = (PlayerMovementV3.tempSpeed * 100) + " MPH";
    }
}
