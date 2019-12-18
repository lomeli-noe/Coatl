using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreWindow : MonoBehaviour
{
    private Text scoreText;

    private void Awake()
    {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();

        Score.OnHighScoreChanged += Score_OnHighScoreChanged;

        UpdateHighScore();
    }

    private void Score_OnHighScoreChanged(object sender, System.EventArgs e)
    {
        UpdateHighScore();
    }

    private void Update()
    {
        scoreText.text = "Score: " + Score.GetScore().ToString();
    }

    private void UpdateHighScore()
    {
        int highscore = Score.GetHighScore();
        transform.Find("HighScoreText").GetComponent<Text>().text = "High Score: " + highscore.ToString();
    }
}
