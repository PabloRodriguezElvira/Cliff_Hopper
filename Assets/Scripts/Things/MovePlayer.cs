using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using Unity.VisualScripting;
using UnityEngine;


//enum MoveDirection { LEFT, FORWARD, RIGHT };


public class MovePlayer : MonoBehaviour
{
    //Para obtener las placas de level.
    private List<Vector3> Placas;

    public AudioClip jumpSound;
    bool bMoving;
    bool canRotate;
    Vector3 actualPos;
    float angle;
    int index_placa;
    Vector3 currentDirection, moveDirection;

	private void Awake()
	{
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
	}
	private void OnDestroy()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	}

	private void GameManager_OnGameStateChanged(GameState state)
	{
		print("tumadretieneunapolllaaa");
        if (state == GameState.Play) iniciarPlayer();
	}
  
    private void iniciarPlayer()
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
            //Si pisamos una placa pintada, tenemos que ir en dirección a la siguiente.
            canRotate = true;
            ++index_placa;
        }
    }

    void Update()
    {
        if (GameManager.Instance.State == GameState.Play)
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

				//Cambiamos dirección.
				moveDirection = (Placas[index_placa] - actualPos).normalized;

				//Rotamos player.
				transform.rotation = Quaternion.LookRotation(moveDirection);

				currentDirection = moveDirection;
				AudioSource.PlayClipAtPoint(jumpSound, transform.position);
			}
			if (bMoving)
			{
				//Desplazamiento
				transform.position = transform.position + (1.0f/40.0f) * moveDirection;


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

			//Presionar escape -> Volver al Menu. (Implementar Menu de Pausa).
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				bMoving = false;
				//Cambiar estado a menu.
				GameManager.Instance.ChangeState(GameState.Menu);
			}

        }
    }
}
