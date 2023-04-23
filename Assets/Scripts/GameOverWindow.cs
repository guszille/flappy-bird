using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI hightScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button mainMenuButton;

    private void Awake()
    {
        retryButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.GameScene);
        });

        mainMenuButton.onClick.AddListener(() =>
        {
            Loader.Load(Loader.Scene.MainMenu);
        });
    }

    private void Start()
    {
        BirdController.Instance.OnDied += Bird_OnDied;

        Hide();
    }

    private void Bird_OnDied(object sender, System.EventArgs args)
    {
        hightScoreText.text = "HIGH SOCORE: " + ScoreController.GetHightScore().ToString();
        scoreText.text = "SCORE: " + LevelController.Instance.GetPipesPassedCount().ToString();

        Show();
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
