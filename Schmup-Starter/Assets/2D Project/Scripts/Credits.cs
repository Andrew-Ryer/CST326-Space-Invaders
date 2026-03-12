using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(returnCredits());
    }
    
    private IEnumerator returnCredits()
    {
        yield return new WaitForSecondsRealtime(5);
        SceneManager.LoadScene("2D Project/Scenes/Startup");
    }
}
