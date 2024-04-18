using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class UIManagerV2 : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private TMP_Text scoreText, pauseScoreText, highScoreText;
    [SerializeField] private ScoreMechanics scoreMechanics;
    [SerializeField] private Slider hpBar;

    private void Start()
    {
        InitializeHpBar();
    }

    private void OnEnable()
    {
        SubscribeToEvents();
    }

    private void InitializeHpBar()
    {
        hpBar = FindObjectOfType<Slider>();

        if (hpBar != null)
        {
            hpBar.value = PlayerV2.Hp;
        }
        else
        {
            Debug.LogError("Hp Slider not found!");
        }
    }

    private void SubscribeToEvents()
    {
        EventManager.OnHpChanged += UpdateHp;
        EventManager.OnEnemyKilled += UpdateScoreText;
    }

    private void UpdateHp()
    {
        if (hpBar == null) return;

        hpBar.value = (float)PlayerV2.Hp / 100;
        
        if (hpBar.value <= 0 || Input.GetKeyDown(KeyCode.Z))
        {
            ShowDeadMenu();
        }
    }

    private void UpdateScoreText()
    {
        scoreText.text = $"Score: {scoreMechanics.score}";
    }

    private void ShowDeadMenu()
    {
        if (pauseMenu == null) 
        {
            Debug.LogWarning("Pause Menu is null");
            return;
        }

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
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
