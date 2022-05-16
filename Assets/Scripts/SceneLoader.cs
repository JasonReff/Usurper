using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadScene(String sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}