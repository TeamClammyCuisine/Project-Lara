using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public GameObject Crack;
    public GameObject MainMenu;
    public GameObject Options;
    public GameObject Help;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MenuPlay()
    {
        Crack.SetActive(true);
        StartCoroutine("DelayedStartGame");
    }

    IEnumerator DelayedStartGame()
    {
        yield return new WaitForSeconds(0.05f);
        SceneManager.LoadScene("demoScene", LoadSceneMode.Single);
    }

    public void MenuOptions()
    {
        MainMenu.SetActive(false);
        Options.SetActive(true);
    }

    public void MenuHelp()
    {
        MainMenu.SetActive(false);
        Help.SetActive(true);
    }

    public void MenuQuit()
    {
        Application.Quit();
    }

    public void OptionsQuit()
    {
        Options.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void HelpQuit()
    {
        Help.SetActive(false);
        MainMenu.SetActive(true);
    }
}
