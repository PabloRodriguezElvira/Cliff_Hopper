using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


enum MoveDirection { LEFT, FORWARD, RIGHT };


public class MovePlayer : MonoBehaviour
{
    public AudioClip jumpSound;

    bool bMoving;
    Vector3 initPos;
    float angle;
    MoveDirection currentDirection, moveDirection;

    void Start()
    {
        bMoving = false;
        currentDirection = MoveDirection.FORWARD;
    }

    void Update()
    {
        if (!bMoving && (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)))
        {
            bMoving = true;
            initPos = transform.position;
            angle = 0;
            moveDirection = MoveDirection.FORWARD;
            if (currentDirection == MoveDirection.LEFT)
                transform.Rotate(0.0f, 90.0f, 0.0f);
            else if(currentDirection == MoveDirection.RIGHT)
                transform.Rotate(0.0f, -90.0f, 0.0f);
            currentDirection = moveDirection;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        else if(!bMoving && (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)))
        {
            bMoving = true;
            initPos = transform.position;
            angle = 0;
            moveDirection = MoveDirection.LEFT;
            if (currentDirection == MoveDirection.FORWARD)
                transform.Rotate(0.0f, -90.0f, 0.0f);
            else if (currentDirection == MoveDirection.RIGHT)
                transform.Rotate(0.0f, -180.0f, 0.0f);
            currentDirection = moveDirection;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position, 2.0f);
        }
        else if (!bMoving && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)))
        {
            bMoving = true;
            initPos = transform.position;
            angle = 0;
            moveDirection = MoveDirection.RIGHT;
            if (currentDirection == MoveDirection.LEFT)
                transform.Rotate(0.0f, 180.0f, 0.0f);
            else if (currentDirection == MoveDirection.FORWARD)
                transform.Rotate(0.0f, 90.0f, 0.0f);
            currentDirection = moveDirection;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        if (bMoving)
        {
            switch (moveDirection)
            {
                case MoveDirection.LEFT:
                    transform.position = initPos + new Vector3(-angle / 180.0f, 0.5f * Mathf.Sin(angle * Mathf.PI / 180.0f), 0.0f);
                    break;
                case MoveDirection.FORWARD:
                    transform.position = initPos + new Vector3(0.0f, 0.5f * Mathf.Sin(angle * Mathf.PI / 180.0f), angle / 180.0f);
                    break;
                case MoveDirection.RIGHT:
                    transform.position = initPos + new Vector3(angle / 180.0f, 0.5f * Mathf.Sin(angle * Mathf.PI / 180.0f), 0.0f);
                    break;
            }
            angle += Time.deltaTime * 1000.0f;
            if (angle >= 180.0f)
            {
                bMoving = false;
                switch (moveDirection)
                {
                    case MoveDirection.LEFT:
                        transform.position = initPos + new Vector3(-1.0f, 0.0f, 0.0f);
                        break;
                    case MoveDirection.FORWARD:
                        transform.position = initPos + new Vector3(0.0f, 0.0f, 1.0f);
                        break;
                    case MoveDirection.RIGHT:
                        transform.position = initPos + new Vector3(1.0f, 0.0f, 0.0f);
                        break;
                }
            }
        }
    }
}
