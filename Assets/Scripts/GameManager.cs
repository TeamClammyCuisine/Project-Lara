using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    private void Update()
    {
        
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void GiveUp()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
