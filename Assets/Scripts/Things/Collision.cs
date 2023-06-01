using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    public GameObject bloquePintado;
    void OnTriggerEnter(Collider objeto)
    {

        if (objeto.gameObject.CompareTag("Placa")) 
        {
            objeto.gameObject.SetActive(false);
            Instantiate(bloquePintado, objeto.transform.position, objeto.transform.rotation); 
        }

        Debug.Log("Colisio = " + objeto.gameObject.name);
    }
}
