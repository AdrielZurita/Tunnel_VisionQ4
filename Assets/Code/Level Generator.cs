using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    public GameObject[] levelParts; 
    int randomIndex = -1;
    int previousIndex = -1;
    Transform startTransform;
    Transform endTransform;
    public float currentDistance = 35f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GenerateLevel(int cellCount)
    {
        for (int i = 0; i < cellCount; i++)
            {   
                randomIndex = Random.Range(0, levelParts.Length);
                if (previousIndex == randomIndex)
                {
                    randomIndex = (randomIndex + 1) % levelParts.Length;
                }
                GameObject levelPart = Instantiate(levelParts[randomIndex]);
                endTransform = levelPart.transform.Find("end");
                startTransform = levelPart.transform.Find("start");
                currentDistance += endTransform.position.z - startTransform.position.z;
                levelPart.transform.position = new Vector3(0, 0, currentDistance);
                previousIndex = randomIndex;
            }
    }
}
