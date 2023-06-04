using System;
using UnityEngine;

public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnGameStateChanged;
    
    //HUD:
    [SerializeField] private GameObject hud;

    //Player:
    [SerializeField] private GameObject player;

    //Lose window:
    [SerializeField] private GameObject loseDialog;

    //Win window:
    [SerializeField] private GameObject winDialog;

    //Highscore:
    [SerializeField] private int highscore;

    //Audio Source:
    [SerializeField] private AudioSource musicSource;

    //Menu Theme:
    [SerializeField] private AudioClip menuTheme;

    //Game Theme:
    [SerializeField] private AudioClip gameTheme;

    public GameState State { get; private set; }

    void Start() => ChangeState(GameState.Menu);

    public void ChangeState(GameState newState) {
        OnGameStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Menu:
                HandleMenu();
                break;
            case GameState.PauseMenu:
                HandlePauseMenu();
                break;
            case GameState.Play:
                HandlePlay();
                break;
            case GameState.Lose:
                HandleLose();
                break;
            case GameState.Win:
                HandleWin();
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        } 
    }

    private void HandlePlay()
    {
        //Desactivar Menu;
        MenuManager.instance.currentState.SetActive(false);

        //Activar HUD y desactivar lose window:
        hud.SetActive(true);
        loseDialog.SetActive(false);

        musicSource.volume = 0.025f;
        musicSource.clip = gameTheme;
        musicSource.Play();
    }

	private void HandleMenu()
    {    
        //Crear Nivel:
        Level.instance.crearNivel();

        //Reset Player
        player.GetComponent<Player>().resetPlayer();

        //Desactivar HUD, lose y win window:
        hud.SetActive(false);
        loseDialog.SetActive(false);
        winDialog.SetActive(false);

        musicSource.volume = 0.05f;
        musicSource.clip = menuTheme;
        musicSource.Play();
    }
    
    private void HandleLose()
    {
        //Fog roja:
        Level.instance.changeFogColor();
        //Desactivar HUD y win window:
        hud.SetActive(false);
        winDialog.SetActive(false);
        //Show lose:
        loseDialog.SetActive(true);
    }

    private void HandleWin()
    {
        //Desactivar HUD y lose window:
        hud.SetActive(true);
        loseDialog.SetActive(false);
        //Show win:
        winDialog.SetActive(true);
    }

    private void HandlePauseMenu()
    {
    }
 
}

[Serializable]
public enum GameState {
    Starting = 0,
    Menu = 1,
    PauseMenu = 2,
    Play = 3,
    Win = 4,
    Lose = 5
}