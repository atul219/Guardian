using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void RelaodGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;
    }

    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1;

    }
    // To exit the game
    public void ExitGame()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;
        }
        
        else
        {
            Application.Quit();
        }
    }
}
