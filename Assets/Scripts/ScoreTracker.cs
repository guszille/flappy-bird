using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Start()
    {
        LevelController.Instance.OnPipesPassedCountChanged += LevelController_OnPipesPassedCountChanged;

        highScoreText.text = "HIGH SCORE: " + ScoreController.GetHightScore().ToString();
    }

    private void LevelController_OnPipesPassedCountChanged(object sender, LevelController.OnPipesPassedCountChangedEventArgs args)
    {
        scoreText.text = args.pipesPassedCount.ToString();
    }
}
