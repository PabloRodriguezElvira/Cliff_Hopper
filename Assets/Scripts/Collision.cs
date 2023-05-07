using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collision : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //Destroy(gameObject);
        SceneManager.LoadScene("GameScene");
        Debug.Log("Colisio = " + other.gameObject.name);
    }
}
