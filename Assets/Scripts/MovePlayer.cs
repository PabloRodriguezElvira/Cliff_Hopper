using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;


enum MoveDirection { LEFT, FORWARD, RIGHT };


public class MovePlayer : MonoBehaviour
{
    public AudioClip jumpSound;

    bool bMoving;
    bool canRotate;
    Vector3 initPos;
    float angle;
    MoveDirection currentDirection, moveDirection;

    void Start()
    {
        bMoving = false;
        canRotate = false;
        currentDirection = MoveDirection.FORWARD;
        moveDirection = MoveDirection.FORWARD;
    }

    void OnTriggerEnter(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("PlacaPintada"))
        {
            canRotate = true; 
        } 
    }

    void Update()
    {
        //Moverse palante.
        if (!bMoving)
        {
            bMoving = true;
            initPos = transform.position;
            angle = 0;

            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        else if (canRotate && (Input.GetKey(KeyCode.Space)))
        {
            bMoving = true;
            canRotate = false;
            initPos = transform.position;
            angle = 0;

            //Cambiamos dirección.
            if (moveDirection == MoveDirection.FORWARD) moveDirection = MoveDirection.RIGHT;
            else if (moveDirection == MoveDirection.RIGHT) moveDirection = MoveDirection.FORWARD; 

            //Rotamos player.
            if (currentDirection == MoveDirection.FORWARD)
                transform.Rotate(0.0f, 90.0f, 0.0f);
            if (currentDirection == MoveDirection.RIGHT)
                transform.Rotate(0.0f, -90.0f, 0.0f);

            currentDirection = moveDirection;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        //else if (canRotate && (Input.GetKey(KeyCode.Space)))
        //{
        //    bMoving = true;
        //    canRotate = false;
        //    initPos = transform.position;
        //    angle = 0;
        //    moveDirection = MoveDirection.LEFT;
        //    if (currentDirection == MoveDirection.FORWARD)
        //        transform.Rotate(0.0f, -90.0f, 0.0f);
        //    else if (currentDirection == MoveDirection.RIGHT)
        //        transform.Rotate(0.0f, -180.0f, 0.0f);
        //    currentDirection = moveDirection;
        //    AudioSource.PlayClipAtPoint(jumpSound, transform.position, 2.0f);
        //}
        if (bMoving)
        {
            //Animación de saltito:
            //switch (moveDirection)
            //{
            //    case MoveDirection.LEFT:
            //        transform.position = initPos + new Vector3(-angle / 180.0f, 0.5f * Mathf.Sin(angle * Mathf.PI / 180.0f), 0.0f);
            //        break;
            //    case MoveDirection.FORWARD:
            //        transform.position = initPos + new Vector3(0.0f, 0.5f * Mathf.Sin(angle * Mathf.PI / 180.0f), angle / 180.0f);
            //        break;
            //    case MoveDirection.RIGHT:
            //        transform.position = initPos + new Vector3(angle / 180.0f, 0.5f * Mathf.Sin(angle * Mathf.PI / 180.0f), 0.0f);
            //        break;
            //}
            //Desplazamiento
            angle += (Time.deltaTime * 1000.0f)/1.5f;
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
