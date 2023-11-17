using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [Tooltip("The main menu scene.")]
    [SerializeField] private string _mainMenu;

    void Start()
    {
        gameObject.active = false;
    }

    public void Activate()
    {
        gameObject.active = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit()
    {
        SceneManager.LoadScene(_mainMenu);
    }
}
