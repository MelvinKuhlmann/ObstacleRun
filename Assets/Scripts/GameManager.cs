using UnityEngine;

public enum GameState { MAIN_MENU, LEVEL_SELECT, PAUSED, GAME }
public class GameManager
{
    public GameState gameState { get; private set; }

    #region Singleton
    private static GameManager instance;

    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new GameManager();
            }
            return instance;
        }
    }
    #endregion

    public void SetGameState(GameState state)
    {
        gameState = state;
        if (gameState == GameState.PAUSED)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public void Pause(bool paused)
    {
        if (paused)
        {
            SetGameState(GameState.PAUSED);
        } else
        {
            SetGameState(GameState.GAME);
        }
    }
}
