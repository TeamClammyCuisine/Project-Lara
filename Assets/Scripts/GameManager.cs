using System;
using System.Collections;
using System.Collections.Generic;
using NpcGen;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoSingleton<GameManager>
{
    public GameObject TryAgainWindow;
    public GameObject Gui;
    public GameObject PoisonBar;
    public GameObject LaraDialogBubble;
    private EnemyGenerator _enemyGenerator;
    public Text EnemyCounter;

    public delegate void GameManagerDelegate();
    public static event GameManagerDelegate UnlockLaraVenom;

    int currentNpcs;
    int currentZone = 1;
    public int EnemiesOnZone1 = 5;
    public int EnemiesOnZone2 = 10;
    public int EnemiesOnZone3 = 15;
    public int EnemiesOnZone4 = 5;

    private void Start()
    {
        currentZone = 1;
        _enemyGenerator.Generate(currentZone, EnemiesOnZone1);
    }
    
    void onLaraDied()
    {
        StartCoroutine(GameOver());
    }

    void OnNpcDied()
    {
        currentNpcs = Int32.Parse(EnemyCounter.text);
        currentNpcs--;
        if (currentNpcs < 1) ProgressZone();
        EnemyCounter.text = currentNpcs.ToString();
    }

    void ProgressZone()
    {
        Debug.Log("Zone Cleared");
        switch (currentZone)
        {
            case 1:
                currentZone = 2;
                UnlockLaraVenom();
                PoisonBar.SetActive(true);
                LaraDialog(1);
                currentNpcs = EnemiesOnZone2;
                EnemyCounter.text = currentNpcs.ToString();
                _enemyGenerator.Generate(currentZone, EnemiesOnZone2);
                
                break;
            case 2:
                currentZone = 3;
                UnlockZone3();
                LaraDialog(2);
                currentNpcs = EnemiesOnZone3;
                EnemyCounter.text = currentNpcs.ToString();
                _enemyGenerator.Generate(currentZone, EnemiesOnZone3);
                break;
            case 3:
                currentZone = 4;
                BossArrives();
                LaraDialog(3);
                currentNpcs = EnemiesOnZone4;
                EnemyCounter.text = currentNpcs.ToString();
                _enemyGenerator.Generate(currentZone, EnemiesOnZone4);
                break;
        }
    }

    void LaraDialog(int index)
    {
        var dialogText = LaraDialogBubble.GetComponent<Text>();
        switch (index)
        {
            case 1:
                LaraDialogBubble.SetActive(true);
                dialogText.text = "I feel strong enough to spit my acidic venom. It can even melt tree strumps";
                StartCoroutine(FadeDialog());
                 break;
            case 2:
                LaraDialogBubble.SetActive(true);
                dialogText.text = "I Heard a loud bang to the west. I should check it out. Still hungry...";
                StartCoroutine(FadeDialog());
                break;
            case 3:
                LaraDialogBubble.SetActive(true);
                dialogText.text = "The king of these men, has arrived at the beach. He'll do as desert.";
                StartCoroutine(FadeDialog());
                break;
        }
   
    }

    IEnumerator FadeDialog()
    {
        yield return new WaitForSeconds(4);
        LaraDialogBubble.SetActive(false);
    }

    void UnlockZone3()
    {


    }

    void BossArrives()
    {

    }

    public void TryAgain()
    {
        SceneManager.LoadScene("DemoScene");
    }

    IEnumerator GameOver()
    {
        yield return new WaitForSeconds(1.2f);
        Instantiate(TryAgainWindow, Gui.transform);
    }

    public void GiveUp()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void OnEnable()
    {
        Player.LaraDied += onLaraDied;
        Npc.NpcDied += OnNpcDied;
    }

    private void OnDisable()
    {
        Player.LaraDied -= onLaraDied;
        Npc.NpcDied -= OnNpcDied;
    }
}
