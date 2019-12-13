using UnityEngine;
using System.Collections.Generic;
using CodeMonkey.Utils;
using CodeMonkey;

public class Snake : MonoBehaviour
{

    private enum Direction
    {
        Left,
        Right,
        Up,
        Down
    }

    private enum State
    {
        Alive,
        Dead
    }

    private State state;
    private Direction gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
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
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;
        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Alive;
    }

    private void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

    }

    private void Update()
    {
        switch(state)
        {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                break;
        }

        if (transform.position.x > 76 || transform.position.x < -76 ||
                    transform.position.y > 39 || transform.position.y < -39)
        {
            state = State.Dead;
        }

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
                            if (gridMoveDirection != Direction.Left)
                            {
                                gridMoveDirection = Direction.Right;

                                transform.localScale = new Vector3(1f, 1f, 0f);
                            }
                        }
                        else
                        {   //Left swipe
                            if (gridMoveDirection != Direction.Right)
                            {
                                gridMoveDirection = Direction.Left;

                                transform.localScale = new Vector3(1f, -1f, 0f);
                            }
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            if (gridMoveDirection != Direction.Down)
                            {
                                gridMoveDirection = Direction.Up;
                            }
                        }
                        else
                        {   //Down swipe
                            if (gridMoveDirection != Direction.Up)
                            {
                                gridMoveDirection = Direction.Down;
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
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if(snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }
            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2Int gridMoveDirectionVector;

            switch (gridMoveDirection)
            {
                default:
                case Direction.Right:   gridMoveDirectionVector = new Vector2Int(1, 0); break;
                case Direction.Left:    gridMoveDirectionVector = new Vector2Int(-1, 0); break;
                case Direction.Up:      gridMoveDirectionVector = new Vector2Int(0, 1); break;
                case Direction.Down:    gridMoveDirectionVector = new Vector2Int(0, -1); break;
            }

            gridPosition += gridMoveDirectionVector;

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

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2Int snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if(gridPosition == snakeBodyPartGridPosition)
                {
                    CMDebug.TextPopup("Dead!!! ", transform.position);
                    state = State.Dead;
                }
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector));
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
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
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
        foreach(SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }

    private class SnakeBodyPart
    {

        private SnakeMovePosition snakeMovePosition;
        private Transform transform;

        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.instance.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection())
            {
                default:

                case Direction.Up:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0; break;
                        case Direction.Left:
                            angle = 45;
                            transform.position = new Vector3(transform.position.x - .2f, transform.position.y - .2f);
                            break;
                        case Direction.Right:
                            angle = -45;
                            transform.position = new Vector3(transform.position.x + .2f, transform.position.y - .2f);
                            break;
                    }
                    break;
                case Direction.Down:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180; break;
                        case Direction.Left:
                            angle = 180 - 45;
                            transform.position = new Vector3(transform.position.x + .2f, transform.position.y - .2f);
                            break;
                        case Direction.Right:
                            angle = 180 + 45;
                            transform.position = new Vector3(transform.position.x - .2f, transform.position.y - .2f);
                            break;
                    }
                    break;
                case Direction.Left:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90; break;
                        case Direction.Down:
                            angle = -45; break;
                        case Direction.Up:
                            angle = 45; break;
                    }
                    break;
                case Direction.Right:
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 90; break;
                        case Direction.Down:
                            angle = 45; break;
                        case Direction.Up:
                            angle = -45; break;
                    }
                    break;
            }
            transform.eulerAngles = new Vector3(0, 0, angle);
        }


        public Vector2Int GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }

    }


    private class SnakeMovePosition
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2Int gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2Int gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2Int GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if(previousSnakeMovePosition == null)
            {
                return Direction.Right;
            }
            return previousSnakeMovePosition.direction;
        }
    }
}
