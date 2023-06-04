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
		// Calcular la posición objetivo de la cámara sumando el offset al jugador
		Vector3 posicionObjetivo = player.transform.position + offset;

		// Utilizar SmoothDamp para suavizar la transición entre la posición actual y la posición objetivo
		Vector3 posicionSuavizada = Vector3.SmoothDamp(transform.position, posicionObjetivo, ref speed, suavidad);

		// Actualizar la posición de la cámara
		transform.position = posicionSuavizada;
    }
}
