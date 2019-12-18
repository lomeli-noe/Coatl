using System;
using UnityEngine;

public class Score
{

    public static event EventHandler OnHighScoreChanged;

    private static int score;

    public static void InitializeStatic()
    {
        OnHighScoreChanged = null;
        score = 0;
    }

    public static int GetScore()
    {
        return score;
    }

    public static void AddScore()
    {
        score += 100;
    }

    public static int GetHighScore()
    {
        return PlayerPrefs.GetInt("highscore", 0);
    }

    public static bool TrySetNewHighScore()
    {
        return TrySetNewHighScore(score);
    }

    public static bool TrySetNewHighScore(int score)
    {
        int highScore = GetHighScore();
        if(score > highScore)
        {
            PlayerPrefs.SetInt("highscore", score);
            PlayerPrefs.Save();
            if (OnHighScoreChanged != null) OnHighScoreChanged(null, EventArgs.Empty);
            return true;
        }
        return false;
    }
}
