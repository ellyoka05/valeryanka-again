using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class PlayerLivesUI : MonoBehaviour
{
    public int maxLives = 5;
    public Image[] lifeImages;
    private float respawnInvulnerabilityTime = 0.25f;

    private int currentLives;
    private FirstPersonController playerController;
    private bool isInvulnerable = false;

    public static event Action OnGameOver;

    void Start()
    {
        playerController = FindObjectOfType<FirstPersonController>();
        currentLives = maxLives;
        UpdateLivesUI();
        FirstPersonController.OnPlayerDeath += HandlePlayerDeath;
    }

    void OnDestroy()
    {
        FirstPersonController.OnPlayerDeath -= HandlePlayerDeath;
    }

    private void HandlePlayerDeath()
    {
        if (isInvulnerable) return;

        currentLives--;
        UpdateLivesUI();

        if (currentLives <= 0)
        {
            print("Game Over!");
            OnGameOver?.Invoke();
            currentLives = maxLives;
        }
        else
        {
            isInvulnerable = true;
            StartCoroutine(InvulnerabilityPeriod());
        }
    }

    private System.Collections.IEnumerator InvulnerabilityPeriod()
    {
        yield return new WaitForSeconds(respawnInvulnerabilityTime);
        isInvulnerable = false;
    }

    private void UpdateLivesUI()
    {
        for (int i = 0; i < lifeImages.Length; i++)
        {
            lifeImages[i].enabled = i < currentLives;
        }
    }
}