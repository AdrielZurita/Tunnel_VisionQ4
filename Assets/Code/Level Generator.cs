using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] levelParts; 
    int randomIndex = -1;
    int previousIndex = -1;
    Transform startTransform;
    [SerializeField] Transform endTransform;
    public Vector3 currentDistance = new Vector3(0f,0f,0f);
    public int RollCount = 2;
    public GameObject startingLevelPart;
    public GameObject endingLevelPart;

    public void GenerateLevel(int cellCount)
    {
        GameObject startingPart = Instantiate(startingLevelPart, currentDistance, Quaternion.Euler(0,0,0)); // creates the starting level part
        endTransform = startingPart.transform.Find("End");
        currentDistance = endTransform.position; // sets currentDistance for later

        for (int i = 0; i < cellCount; i++)
        {   
            randomIndex = Random.Range(0, levelParts.Length); //choses a level part at random

            for (int r = 0; r < RollCount; r++) //decides how many times to roll for a new level part, to avoid dupes
            {
                if (previousIndex == randomIndex) //if its a dupe of the last one, roll again
                {
                    randomIndex = Random.Range(0, levelParts.Length); //rerolls for a new level part
                }
            }

            GameObject levelPart = Instantiate(levelParts[randomIndex], currentDistance, Quaternion.Euler(0,0,0)); // creates the level part at the current distance
            endTransform = levelPart.transform.Find("End");
            currentDistance = endTransform.position; // this is in GLOBAL space so it DOESNT need to be +=
            previousIndex = randomIndex;
        }
        
        GameObject endingPart = Instantiate(endingLevelPart, currentDistance, Quaternion.Euler(0,0,0)); // creates the ending level part
    }
}
