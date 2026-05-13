using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelGenerator levelGeneratorScript;
    public PlayerSettings levelPartCount;

    // Start is called before the first frame update
    void Start()
    {
        levelGeneratorScript.GenerateLevel(levelPartCount.levelPartCount);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
