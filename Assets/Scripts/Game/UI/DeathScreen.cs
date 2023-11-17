using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    #region Singleton Stuff 

    public static DeathScreen Instance = null;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
        }

        Instance = this;
    }

    #endregion

    [Tooltip("The main menu scene.")]
    [SerializeField] private string _mainMenu;
    [Tooltip("The amount of health the player starts with.")]
    [SerializeField] private int _maxHealth;

    private int _health;

    void Start()
    {
        _health = _maxHealth;
        gameObject.SetActive(false);
    }

    public void TakeDamage(int damage = 1)
    {
        _health -= damage;
        if (_health <= 0) Activate();
    }

    private void Activate()
    {
        gameObject.SetActive(true);
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
