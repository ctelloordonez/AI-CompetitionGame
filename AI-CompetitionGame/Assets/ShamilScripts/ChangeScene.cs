using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;



public class ChangeScene : MonoBehaviour
{ 

    public void Start()
    {
       
    }

    public void GameScene(int gameScene)
    {
        SceneManager.LoadScene(gameScene);
       
    }
    public void Quit()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
}
