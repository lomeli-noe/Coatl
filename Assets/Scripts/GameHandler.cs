using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    private static GameHandler instance;

    [SerializeField] private Snake snake;

    private LevelGrid levelGrid;


    private void Awake()
    {
        instance = this;
        Score.InitializeStatic();
        Speed.InitializeStatic();
    }

    private void Start()
    {
        levelGrid = new LevelGrid(10, 20);
        snake.Setup(levelGrid);
        levelGrid.SetUp(snake);
    }

    public static void SnakeDied()
    {
        Score.TrySetNewHighScore();
        GameOverWindow.ShowStatic();
    }
}
