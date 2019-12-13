using UnityEngine;
using System.Collections.Generic;
using CodeMonkey.Utils;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2Int> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;


    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private void Awake()
    {
        gridPosition = new Vector2Int(5, 5);
        gridMoveTimerMax = .1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);

        snakeMovePositionList = new List<Vector2Int>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();
    }

    private void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

    }

    private void Update()
    {
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput()
    {
        if (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            Touch touch = Input.GetTouch(0); // get the touch
            if (touch.phase == TouchPhase.Began) //check for the first touch
            {
                fp = touch.position;
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
            {
                lp = touch.position;
            }
            else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
            {
                lp = touch.position;  //last touch position. Ommitted if you use list

                //Check if drag distance is greater than 20% of the screen height
                if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                {//It's a drag
                 //check if the drag is vertical or horizontal
                    if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                    {   //If the horizontal movement is greater than the vertical movement...
                        if ((lp.x > fp.x))  //If the movement was to the right)
                        {   //Right swipe
                            if (gridMoveDirection.x != -1)
                            {
                                gridMoveDirection.y = 0;
                                gridMoveDirection.x = +1;

                                transform.localScale = new Vector3(1f, 1f, 0f);
                            }
                        }
                        else
                        {   //Left swipe
                            if (gridMoveDirection.x != 1)
                            {
                                gridMoveDirection.y = 0;
                                gridMoveDirection.x = -1;

                                transform.localScale = new Vector3(1f, -1f, 0f);
                            }
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            if (gridMoveDirection.y != -1)
                            {
                                gridMoveDirection.y = +1;
                                gridMoveDirection.x = 0;
                            }
                        }
                        else
                        {   //Down swipe
                            if (gridMoveDirection.y != 1)
                            {
                                gridMoveDirection.y = -1;
                                gridMoveDirection.x = 0;
                            }
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime;
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                snakeBodySize++;
                CreateSnakeBodyPart();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            //for(int i =0; i < snakeMovePositionList.Count; i++)
            //{
            //    Vector2Int snakeMovePosition = snakeMovePositionList[i];
            //    World_Sprite worldSprite =  World_Sprite.Create(new Vector3(snakeMovePosition.x, snakeMovePosition.y), Vector3.one * .1f, Color.white);
            //    FunctionTimer.Create(worldSprite.DestroySelf, gridMoveTimerMax);
            //}

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));

            UpdateSnakeBodyParts();
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2Int dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2Int GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2Int> GetFullSnakeGridPositionList()
    {
        List<Vector2Int> gridPositionList = new List<Vector2Int>() { gridPosition };
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }

    private class SnakeBodyPart
    {

        private Vector2Int gridPosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(Vector2Int gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }
}
