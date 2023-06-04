using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


//enum MoveDirection { LEFT, FORWARD, RIGHT };


public class MovePlayer : MonoBehaviour
{	
    //Para obtener las placas de level.
    private List<Vector3> Placas;

	//Monedas:
	public int coins;
	public TextMeshProUGUI coinsDisplay;
	
	//Giros:
	public int giros;
	public TextMeshProUGUI girosDisplay;

	public GameObject bloquePintado;
	

    public AudioClip jumpSound;
    bool bMoving;
    bool canRotate;
    Vector3 actualPos;
    float angle;
    int index_placa;
    Vector3 currentDirection, moveDirection;

	//private void Awake()
	//{
	//	GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
	//}

	//private void OnDestroy()
	//{
	//	GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	//}

	//private void GameManager_OnGameStateChanged(GameState state)
	//{
	//	if (state == GameState.Play) iniciarPlayer();
	//}

	public void setCoins(int c) { coins = c; }
	public void setGiros(int g) { giros = g; }
  
	private void Start()
    {
		//Parametros de player.
		//No pilla las placas porque el nivel aun no se ha creado(¿?)
        Placas = GameObject.Find("Level").GetComponent<CreateLevel>().Placas;
		resetPlayer();
	}

	public void resetPlayer()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
        bMoving = false;
        canRotate = false;
        index_placa = 0;
        moveDirection = Vector3.forward; 
        currentDirection = moveDirection;
	}

    void OnTriggerEnter(Collider objeto)
    {
		if (objeto.gameObject.CompareTag("Placa"))
		{
			//Si pisamos una placa pintada, tenemos que ir en dirección a la siguiente.
			canRotate = true;
			++index_placa;
			
			//Cambiamos de bloque a bloque pintado.
			Vector3 posBloque = objeto.transform.position;
			Destroy(objeto.gameObject);
			Transform lvlTransform = GameObject.FindGameObjectWithTag("Level").transform;
			Instantiate(bloquePintado, posBloque, Quaternion.identity, lvlTransform);
		}
		else if (objeto.gameObject.CompareTag("pellet"))
		{
			++coins;
		}
	}

	void Update()
    {
        if (GameManager.Instance.State == GameState.Play)
        {
			Debug.Log(transform.position.y);
			//Actualizar coins y giros:
			coinsDisplay.text = coins.ToString();
			girosDisplay.text = giros.ToString();

			
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
				++giros;
				bMoving = true;
				canRotate = false;
				actualPos = transform.position;
				angle = 0;

				//Cambiamos dirección.
				//moveDirection = (Placas[index_placa] - actualPos).normalized;
				moveDirection = Vector3.right;

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

			//Presionar escape -> Menu Pausa
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				bMoving = false;
				//Cambiar estado a menu de pausa.
				GameManager.Instance.ChangeState(GameState.PauseMenu);
			}
			//Perder.
			if (transform.position.y <= -4.0f)
			{
				GameManager.Instance.ChangeState(GameState.Lose);
			}

        }
    }
}
