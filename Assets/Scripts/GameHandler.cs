using UnityEngine;
using CodeMonkey;
using CodeMonkey.Utils;

public class GameHandler : MonoBehaviour
{

    void Start()
    {
        GameObject snakeHeadGameObject = new GameObject();
        SpriteRenderer snakeSpriteRenderer = snakeHeadGameObject.AddComponent<SpriteRenderer>();
        snakeSpriteRenderer.sprite = GameAssets.instance.snakeHeadSprite; 

    }
}
