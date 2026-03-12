using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{

    public void loadScene()
    {
        // To Main Game
        SceneManager.LoadScene("2D Project/Scenes/SpaceInvader");
    }

    public void loadScene3()
    {
        // To Score Table
        SceneManager.LoadScene("2D Project/Scenes/ScoreTable");
    }

    public void loadScene2()
    {
        // To Credits
        SceneManager.LoadScene("2D Project/Scenes/Credits");
    }
    
    public void loadStart()
    {
        // To Main Menu
        SceneManager.LoadScene("2D Project/Scenes/Startup");
    }
}
