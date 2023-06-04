using System;
using UnityEngine;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnGameStateChanged;
    
    //HUD:
    [SerializeField] private GameObject hud;

    //Player:
    [SerializeField] private GameObject player;

    //Lose window:
    [SerializeField] private GameObject loseDialog;

    //Camera:
    [SerializeField] private GameObject camera;

    //Highscore:
    [SerializeField] private int highscore;

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
            //case GameState.SpawningEnemies:
            //    HandleSpawningEnemies();
            //    break;
            //case GameState.HeroTurn:
            //    HandleHeroTurn();
            //    break;
            //case GameState.EnemyTurn:
            //    break;
            //case GameState.Win:
            //    break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        } 
        Debug.Log($"New state: {newState}");
    }

    private void HandlePlay()
    {
        //Desactivar Menu;
        MenuManager.instance.currentState.SetActive(false);

        //Activar HUD y desactivar lose window:
        hud.SetActive(true);
        loseDialog.SetActive(false);
    }

	private void HandleMenu()
    {    
        //Crear Nivel:
        Level.instance.crearNivel();

        //Reset Player
        player.GetComponent<Player>().resetPlayer();

        //Desactivar HUD y lose window:
        hud.SetActive(false);
        loseDialog.SetActive(false);
    }
    
    private void HandleLose()
    {
        //Fog roja:
        Level.instance.changeFogColor();
        //Desactivar HUD:
        hud.SetActive(false);
        //Show lose:
        loseDialog.SetActive(true);
        //Stop camera:
        //camera.GetComponent<Move>().speed = Vector3.zero;
    }

    private void HandlePauseMenu()
    {
    }
 
}

/// <summary>
/// This is obviously an example and I have no idea what kind of game you're making.
/// You can use a similar manager for controlling your menu states or dynamic-cinematics, etc
/// </summary>
[Serializable]
public enum GameState {
    Starting = 0,
    Menu = 1,
    PauseMenu = 2,
    Play = 3,
    Win = 4,
    Lose = 5
}