using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Player : MonoBehaviour
{
    public AudioClip jumpSound;
    bool bMoving, bJumping;

    float speed, jumpForce;
    bool canRotate, rotated;
    Vector3 currentDestination, nextDestination;

    //Monedas:
    public int coins;
    public TextMeshProUGUI coinsDisplay;

    //Giros:
    public int turns;
    public TextMeshProUGUI turnsDisplay;

    void Start()
    {
        resetPlayer();
    }

    public void resetPlayer()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;

        bMoving = false;
        bJumping = false;

        speed = 10.0f;
        jumpForce = 8.0f;
        canRotate = false;
        rotated = false;
        currentDestination = transform.position + 3 * 4 * Vector3.forward;
        currentDestination += (currentDestination - transform.position);

        coins = 0;
        turns = 0;
    }

    void Update()
    {
        if (GameManager.Instance.State == GameState.Play)
        {
            coinsDisplay.text = coins.ToString();
            turnsDisplay.text = turns.ToString();

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                bMoving = false;
                //Cambiar estado a menu de pausa.
                GameManager.Instance.ChangeState(GameState.PauseMenu);
            }

            if (bMoving)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    if (canRotate)
                    {
                        canRotate = false;
                        rotated = true;

                        //Cambiamos dirección.
                        currentDestination = 2 * nextDestination - transform.position;

                        //Rotamos player.
                        transform.rotation = Quaternion.LookRotation(currentDestination);

                        ++turns;

                        AudioSource.PlayClipAtPoint(jumpSound, transform.position);
                    }
                    else if (!bJumping)
                    {
                        bJumping = true;
                        //GetComponent<Rigidbody>().AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                        Debug.Log("jumpForce: " + jumpForce);
                    }
                }




                //Desplazamiento
                transform.position = Vector3.MoveTowards(transform.position, currentDestination, speed * Time.deltaTime);


                //Desplazamiento
                //angle += (Time.deltaTime * 1000.0f)/1.5f;
                //if (angle >= 180.0f)
                //{
                //    bMoving = false;
                //    transform.position = transform.position + moveDirection;

                //POR SI SIRVE PARA EL NUEVO PLAYER:
                //actualPos = transform.position;
                //Vector3 targetPos = actualPos + (moveDirection.normalized * angle);
                //Vector3 newPos = Vector3.Lerp(actualPos, targetPos, 0.5f * (Time.deltaTime/1000.0f));
                //transform.position = newPos;
                //}
            }
            else
            {
                bMoving = true;
                AudioSource.PlayClipAtPoint(jumpSound, transform.position);
            }
        }
    }


        

    //Collision
    [SerializeField] private GameObject TurnedBlock;

    void OnTriggerEnter(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("Coin"))
        {
            Destroy(objeto.gameObject);

            ++coins;
        }
        else
        {
            bJumping = false;

            if (objeto.gameObject.CompareTag("TurnBlock"))
            {
                //Si pisamos un TurnBlock, almacenamos y permitimos ir en dirección a la siguiente.
                canRotate = true;

                nextDestination = objeto.gameObject.GetComponent<TurnBlock>().getNextDestination();
            }
        }
    }

    void OnTriggerExit(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("TurnBlock"))
        {
            //Si dejamos de pisar un TurnBlock, dejamos de permitir ir en dirección a la siguiente.
            canRotate = false;
        }
    }

    void OnTriggerStay(Collider objeto)
    {
        if (rotated && objeto.gameObject.CompareTag("TurnBlock"))
        {
            //Si estamos en un TurnBlock y giramos, lo desactivamos y spawneamos TurnedBlock.
            rotated = false;

            Vector3 posBloque = objeto.transform.position;
            Destroy(objeto.gameObject);
            Transform lvlTransform = GameObject.FindGameObjectWithTag("Level").transform;
            Instantiate(TurnedBlock, posBloque, Quaternion.identity, lvlTransform);
        }
    }
}