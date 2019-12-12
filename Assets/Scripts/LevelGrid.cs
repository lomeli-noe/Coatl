using UnityEngine;
using CodeMonkey.Utils;

public class LevelGrid 
{
    private Vector2Int foodGripPosition;
    private int width;
    private int height;

    public LevelGrid(int width, int height)
    {
        this.width = width;
        this.height = height;

        SpawnFood();
        FunctionPeriodic.Create(SpawnFood, 1f);
    }

    private void SpawnFood()
    {
        foodGripPosition = new Vector2Int(Random.Range(0, width), Random.Range(0, height));

        GameObject foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.foodSprite;
        foodGameObject.transform.position = new Vector3(foodGripPosition.x, foodGripPosition.y);
    }
}
