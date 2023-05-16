using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;


//enum MoveDirection { LEFT, FORWARD, RIGHT };


public class MovePlayer : MonoBehaviour
{
    //Para obtener las placas de level.
    private Vector3[] Placas;


    public AudioClip jumpSound;
    bool bMoving;
    bool canRotate;
    Vector3 actualPos;
    float angle;
    int index_placa;
    Vector3 currentDirection, moveDirection;

    void Start()
    {
        Placas = GameObject.Find("Level").GetComponent<CreateLevel>().Placas;
        bMoving = false;
        canRotate = false;
        index_placa = 0;
        moveDirection = Vector3.forward; 
        currentDirection = moveDirection;
    }

    void OnTriggerEnter(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("PlacaPintada"))
        {
            //Si pisamos una placa pintada, tenemos que ir en direcci�n a la siguiente.
            canRotate = true;
            ++index_placa;
        } 
        if (objeto.gameObject.CompareTag("bloque"))
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
            angle = 0;
            actualPos = transform.position;
            AudioSource.PlayClipAtPoint(jumpSound, actualPos);
        }
        else if (canRotate && (Input.GetKey(KeyCode.Space)))
        {
            bMoving = true;
            canRotate = false;
            actualPos = transform.position;
            angle = 0;

            Debug.Log("IndexPlaca");
            Debug.Log(index_placa);
            //Cambiamos direcci�n.
            moveDirection = (Placas[index_placa] - actualPos).normalized;

            Debug.Log("MoveDir");
            Debug.Log(moveDirection);

            //Rotamos player.
            if (currentDirection == Vector3.forward) transform.Rotate(0.0f, 90.0f, 0.0f);
            else transform.Rotate(0.0f, -90.0f, 0.0f);

            currentDirection = moveDirection;
            AudioSource.PlayClipAtPoint(jumpSound, transform.position);
        }
        if (bMoving)
        {
            //Desplazamiento
            angle += (Time.deltaTime * 1000.0f)/1.5f;
            if (angle >= 180.0f)
            {
                bMoving = false;
                //actualPos = transform.position;
                //Vector3 targetPos = actualPos + (moveDirection.normalized * angle);
                //Vector3 newPos = Vector3.Lerp(actualPos, targetPos, 0.5f * (Time.deltaTime/1000.0f));
                //transform.position = newPos;
                transform.position = transform.position + moveDirection;
                transform.position = new Vector3(transform.position.x, 0.4f, transform.position.z);
            }
        }
    }
}
