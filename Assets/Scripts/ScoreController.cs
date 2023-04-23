using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScoreController
{
    private const string HIGH_SCORE_KEY_NAME = "HighScore";

    public static int GetHightScore()
    {
        return PlayerPrefs.GetInt(HIGH_SCORE_KEY_NAME);
    }

    public static bool TrySetNewHighScore(int score)
    {
        int currentHighScore = GetHightScore();

        if (currentHighScore < score)
        {
            PlayerPrefs.SetInt(HIGH_SCORE_KEY_NAME, score);
            PlayerPrefs.Save();

            return true;
        }

        return false;
    }

    public static void ResetHighScore()
    {
        PlayerPrefs.SetInt(HIGH_SCORE_KEY_NAME, 0);
        PlayerPrefs.Save();
    }
}
