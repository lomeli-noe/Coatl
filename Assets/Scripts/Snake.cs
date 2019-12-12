using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridMoveDirection;
    private Vector2Int gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;

    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered

    private void Awake()
    {
        gridPosition = new Vector2Int(5, 5);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2Int(1, 0);
    }

    private void Start()
    {
        dragDistance = Screen.height * 15 / 100; //dragDistance is 15% height of the screen

    }

    private void Update()
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
                            gridMoveDirection.y = 0;
                            gridMoveDirection.x = +1;
                        }
                        else
                        {   //Left swipe
                            gridMoveDirection.y = 0;
                            gridMoveDirection.x = -1;
                        }
                    }
                    else
                    {   //the vertical movement is greater than the horizontal movement
                        if (lp.y > fp.y)  //If the movement was up
                        {   //Up swipe
                            gridMoveDirection.y = +1;
                            gridMoveDirection.x = 0;
                        }
                        else
                        {   //Down swipe
                            gridMoveDirection.y = -1;
                            gridMoveDirection.x = 0;
                        }
                    }
                }
                else
                {   //It's a tap as the drag distance is less than 20% of the screen height
                    Debug.Log("Tap");
                }
            }
        }

        gridMoveTimer += Time.deltaTime;
        if(gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
        }

        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }
}
