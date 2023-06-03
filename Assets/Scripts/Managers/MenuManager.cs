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
		instructionsPage,
		confirmationDialog
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
				if (!currentState.CompareTag("mainmenu")) SetState(menuState.mainMenu);
				else SetState(menuState.confirmationDialog);	
			}
			else if (Input.GetKeyDown(KeyCode.Space))
			{
				//Solo podemos acceder al juego si estamos en el menú principal.
				if (currentState.CompareTag("mainmenu")) GameManager.Instance.ChangeState(GameState.Play);
			}
		}
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
			case menuState.confirmationDialog:
				currentState = states[3];
				break;
		}
		currentState.SetActive(true);
	}


	public void changeToCredits()
	{
		SetState(menuState.creditsPage);
	}

	public void changeToInfo()
	{
		SetState(menuState.instructionsPage);
	}

	public void changeToMainMenu()
	{
		SetState(menuState.mainMenu);
	}

	public void changeToLeaderboard()
	{

	}

	public void exitDialog()
	{
		SetState(menuState.confirmationDialog);
	}

	public void exitGame()
	{
		Debug.Log("BYE BYE!");
		Application.Quit();
	}
	
}
