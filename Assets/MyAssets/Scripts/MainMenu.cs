using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] LevelLoader levelLoader;

    public void OnPlayClick()
    {
        levelLoader.LoadNextLevel();
    }
    public void OnQuitClick()
    {
        Application.Quit();
    }
}