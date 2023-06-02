using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
	public static MenuManager instance;

	private enum menuState
	{
		mainMenu,
		creditsPage,
		instructionsPage
	}

	public List<GameObject> states;

	public GameObject currentState = null;

	private void Awake()
	{
		instance = this;
		GameManager.OnGameStateChanged += GameManager_OnGameStateChanged;
	}

	private void OnDestroy()
	{
		GameManager.OnGameStateChanged -= GameManager_OnGameStateChanged;
	}

	private void GameManager_OnGameStateChanged(GameState state)
	{
		if (state == GameState.Menu) SetState(menuState.mainMenu);
	}


	private void Update()
	{
		if (GameManager.Instance.State == GameState.Menu)
		{ 
			if (Input.GetKeyDown(KeyCode.Escape))
			{
				SetState(menuState.mainMenu);
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				GameManager.Instance.ChangeState(GameState.Play);
			}
		}
	}

	public void changeToCredits()
	{
		SetState(menuState.creditsPage);
	}

	private void SetState(menuState st)
	{
		//Desactivar estado actual:
		if (currentState != null) currentState.SetActive(false);
		//Cambiar estado:
		switch (st)
		{
			case menuState.mainMenu:
				currentState = states[0];
				break;
			case menuState.creditsPage:
				currentState = states[1];
				break;
			case menuState.instructionsPage:
				currentState = states[2];
				break;
		}
		currentState.SetActive(true);
	}

	public void quitGame()
	{
		Application.Quit();
	}
}
