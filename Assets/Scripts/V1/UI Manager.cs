using System;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Image[] heartImages;
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text scoreText, pauseScoreText, highScoreText;
    [SerializeField] private ScoreMechanics scoreMechanics;
    [SerializeField] private Slider hpSlider;

    private Player player;
    private int currentHearts;

    private void Start()
    {
        player = FindObjectOfType<Player>().GetComponent<Player>();
        InitializeUI();
    }

    private void InitializeUI()
    {
        PrintCurrentScene();
        BindHpSlider();
        currentHearts = heartImages.Length;
        SubscribeToEvents();
    }

    private void PrintCurrentScene() => Debug.Log($"Current Scene: {SceneManager.GetActiveScene().name}");

    private void BindHpSlider()
    {
        if (hpSlider == null)
        {
            Debug.LogError("Hp Slider not found!");
            return;
        }
        hpSlider.value = player.Hp;
    }

    private void SubscribeToEvents()
    {
        EventManager.OnHpChanged += UpdateHp;
        EventManager.OnEnemyKilled += UpdateScoreText;
        EventManager.OutOfScreen += UpdateHearts;
    }

    private void UpdateHearts()
    {
        if (--currentHearts > 0)
        {
            UpdateHeartUIVisibility();
        }
        else
        {
            ShowDeadMenu();
        }
    }

    private void UpdateHeartUIVisibility()
    {
        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].gameObject.SetActive(i < currentHearts);
        }
    }

    private void UpdateHp() => UpdateHpSlider();

    private void UpdateHpSlider()
    {
        if (hpSlider == null)
        {
            Debug.LogWarning("Hp Slider is null");
            return;
        }
        hpSlider.value = (float)player.Hp / 100;
        CheckForDeath();
    }

    private void CheckForDeath()
    {
        if (hpSlider.value <= 0)
        {
            ShowDeadMenu();
        }
    }

    private void UpdateScoreText() => scoreText.text = $"Score: {scoreMechanics.score}";

    private void ShowDeadMenu()
    {
        if (pauseMenu == null)
        {
            Debug.LogWarning("Pause Menu is null");
            return;
        }
        ActivateDeadMenu();
    }

    private void ActivateDeadMenu()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
        UpdateDeadMenuScores();
    }

    private void UpdateDeadMenuScores()
    {
        pauseScoreText.text = scoreMechanics.score.ToString();
        highScoreText.text = scoreMechanics.highScore.ToString();
        pauseMenu.transform.DOScale(Vector3.one, 1f);
    }

    public void RestartScene()
    {
        ResetTimeScale();
        DeactivatePauseMenu();
        LoadCurrentScene();
    }

    private void ResetTimeScale() => Time.timeScale = 1;

    private void DeactivatePauseMenu() => pauseMenu.SetActive(false);

    private void LoadCurrentScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);
}
