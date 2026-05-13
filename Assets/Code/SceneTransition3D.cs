using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition3D : MonoBehaviour
{
    public PlayerSettings livesInfo;
    public string sceneName;
    public string gameOverScene;
    public bool isDeadly;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {   
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (isDeadly)
            {
                livesInfo.currentLives -= 1;
                if (livesInfo.currentLives <= 0)
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(gameOverScene);
                }
                else
                {
                    UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
                }
            }
            else
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
            }
        }
    }
}
