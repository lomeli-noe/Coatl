using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{
    [SerializeField] private Snake snake;
    private LevelGrid levelGrid;

    private void Start()
    {
        levelGrid = new LevelGrid(40, 30);
        snake.Setup(levelGrid);
        levelGrid.SetUp(snake);
    }
}
