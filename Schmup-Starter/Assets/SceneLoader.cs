using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void loadScene()
    {
        SceneManager.LoadScene("2D Project/Scenes/SpaceInvader");
    }

    public void loadScene3()
    {
        SceneManager.LoadScene("2D Project/Scenes/ScoreTable");
    }

    public void loadScene2()
    {
        SceneManager.LoadScene("2D Project/Scenes/Credits");
    }
    
    public void loadStart()
    {
        SceneManager.LoadScene("2D Project/Scenes/Startup");
    }
}
