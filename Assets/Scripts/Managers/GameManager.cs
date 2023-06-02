using System;
using UnityEngine;

/// <summary>
/// Nice, easy to understand enum-based game manager. For larger and more complex games, look into
/// state machines. But this will serve just fine for most games.
/// </summary>
public class GameManager : StaticInstance<GameManager> {
    public static event Action<GameState> OnGameStateChanged;
    //public static event Action<GameState> OnAfterStateChanged;

    public GameState State { get; private set; }

    void Start() => ChangeState(GameState.Starting);

    public void ChangeState(GameState newState) {
        OnGameStateChanged?.Invoke(newState);

        State = newState;
        switch (newState)
        {
            case GameState.Starting:
                HandleStarting();
                break;
            case GameState.Menu:
                HandleMenu();
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

    private void HandleStarting()
    {
        //Crear Nivel:
        CreateLevel.instance.crearNivel();

        //Cambiar estado a Menu:
        ChangeState(GameState.Menu);
    }

    private void HandlePlay()
    {
        //Desactivar Menu;
        MenuManager.instance.currentState.SetActive(false);
    }

	private void HandleMenu()
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
    Play = 2 
}