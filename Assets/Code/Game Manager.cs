using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LevelGenerator levelGeneratorScript;

    // Start is called before the first frame update
    void Start()
    {
        levelGeneratorScript.GenerateLevel(10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
