using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    public GameObject placaPintada;
    void OnTriggerEnter(Collider objeto)
    {
        //Destroy(gameObject);
        //SceneManager.LoadScene("GameScene");
        if (objeto.gameObject.CompareTag("Placa")) 
        {
            //Destroy(objeto);
            objeto.gameObject.SetActive(false);
            Instantiate(placaPintada, objeto.transform.position, objeto.transform.rotation); 
        }
        Debug.Log("Colisio = " + objeto.gameObject.name);

    }
}
