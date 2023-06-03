using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
	public GameObject pauseMenuCanvas;

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
		if (state == GameState.PauseMenu) pauseMenuCanvas.SetActive(true);
	}

	public void Resume()
	{
		pauseMenuCanvas.SetActive(false);
		GameManager.Instance.ChangeState(GameState.Play);
	}
	public void ReturnToMainMenu()
	{
		pauseMenuCanvas.SetActive(false);
		GameManager.Instance.ChangeState(GameState.Menu);
	}
}
