using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject TryAgainWindow;
    public GameObject Gui;

    private void Update()
    {
        
    }

    void onLaraDied()
    {
        Instantiate(TryAgainWindow, Gui.transform);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene("DemoScene");
    }

    public void GiveUp()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEnable()
    {
        Player.LaraDied += onLaraDied;
    }

    private void OnDisable()
    {
        Player.LaraDied -= onLaraDied;
    }
}
