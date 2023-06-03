using System;
using UnityEngine;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnGameStateChanged;
    
    //HUD:
    public GameObject hud;

    //Player:
    public GameObject player;

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
            //case GameState.Lose:
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

        //Activar HUD:
        hud.SetActive(true);
    }

	private void HandleMenu()
    {    
        //Crear Nivel:
        CreateLevel.instance.crearNivel();

        //Reset Player
        player.GetComponent<MovePlayer>().resetPlayer();

        //Desactivar HUD:
        hud.SetActive(false);

        //Reset monedas y giros:
        player.GetComponent<MovePlayer>().setCoins(0);
        player.GetComponent<MovePlayer>().setGiros(0);
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
    Play = 3 
}