using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathHandler : MonoBehaviour
{
    [SerializeField]
    Canvas gameOver;

    private void Start()
    {
        gameOver.enabled = false;
    }

    // if player died
    public void HandleDeath()
    {
        gameOver.enabled = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
