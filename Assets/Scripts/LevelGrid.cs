using UnityEngine;
using CodeMonkey.Utils;

public class LevelGrid 
{
    private Vector2Int foodGripPosition;
    private int width;
    private int height;
    private Snake snake;

    private GameObject foodGameObject;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;
    }

    public void SetUp(Snake snake)
    {
        this.snake = snake;

        SpawnFood();
    }

    private void SpawnFood()
    {
        do
        {
            foodGripPosition = new Vector2Int(Random.Range(-74, 74), Random.Range(-37, 37));
        } while (snake.GetFullSnakeGridPositionList().IndexOf(foodGripPosition) != -1);

        foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGripPosition.x, foodGripPosition.y);
    }

    public bool TrySnakeEatFood(Vector2Int snakeGridPosition)
    {
        if (snakeGridPosition == foodGripPosition)
        {
            Object.Destroy(foodGameObject);
            SpawnFood();
            GameHandler.AddScore();
            return true;
        }
        else
        {
            return false;
        }
    }
}
