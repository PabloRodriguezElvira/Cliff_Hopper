using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class Chomp : MonoBehaviour
{
    public AudioClip jumpSound;
    bool bMoving;

    float speed;
    Vector3 currentDestination, nextDestination, direction, nextCenter;

    void Start()
    {
        resetChomp();
    }

    public void resetChomp()
    {
        transform.position = new Vector3(0.0f, 0.2f, -24.0f);
        transform.rotation = Quaternion.identity;

        bMoving = false;

        speed = 12.0f;

        direction = Vector3.forward;
        currentDestination = transform.position + 9 * 4 * direction;
        nextCenter = currentDestination;
        currentDestination += 16.0f * (currentDestination - transform.position).normalized;
    }

    public void stopChomp()
    {
        bMoving = false;
    }

    void Update()
    {
        if (GameManager.Instance.State == GameState.Play)
        {
            if (bMoving)
            {
                transform.position = Vector3.MoveTowards(transform.position, new Vector3(currentDestination.x, transform.position.y, currentDestination.z), speed * Time.deltaTime);
            }
            else
            {
                bMoving = true;
            }
        }
    }

    void OnTriggerEnter(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("TurnBlock"))
        {
            nextDestination = objeto.gameObject.GetComponent<TurnBlock>().getNextDestination();
        }
        else if (objeto.gameObject.CompareTag("TurnedBlock"))
        {
            nextDestination = objeto.gameObject.GetComponent<TurnedBlock>().getNextDestination();
        }
    }

    void OnTriggerStay(Collider objeto)
    {
        if (objeto.gameObject.CompareTag("TurnBlock") || objeto.gameObject.CompareTag("TurnedBlock"))
        {
            if (direction == Vector3.forward && transform.position.z >= nextCenter.z)
            {
                direction = Vector3.right;
                //transform.position = nextCenter;
                nextCenter = nextDestination;

                //Cambiamos dirección.
                currentDestination = nextDestination + 16.0f * (nextDestination - transform.position).normalized;

                //Rotamos player.
                transform.rotation = Quaternion.LookRotation(currentDestination);
            }
            else if (direction == Vector3.right && transform.position.x >= nextCenter.x)
            {
                direction = Vector3.forward;
                //transform.position = nextCenter;
                nextCenter = nextDestination;

                //Cambiamos dirección.
                currentDestination = nextDestination + 16.0f * (nextDestination - transform.position).normalized;

                //Rotamos player.
                transform.rotation = Quaternion.LookRotation(currentDestination);
            }
        }
    }
}