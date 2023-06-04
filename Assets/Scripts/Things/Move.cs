using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public Vector3 speed;
	public GameObject player;
	public float suavidad;
	public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
		// Calcular la posici�n objetivo de la c�mara sumando el offset al jugador
		Vector3 posicionObjetivo = player.transform.position + offset;

		// Utilizar SmoothDamp para suavizar la transici�n entre la posici�n actual y la posici�n objetivo
		Vector3 posicionSuavizada = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref speed, suavidad);

		// Actualizar la posici�n de la c�mara
		transform.position = posicionSuavizada;
    }
}
