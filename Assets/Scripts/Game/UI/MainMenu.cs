using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Tooltip("The game scene.")]
    [SerializeField] private string _gameScene;

    public void Play()
    {
        SceneManager.LoadScene(_gameScene);
    }

    public void Quit()
    {
        Application.Quit();
    }
}
