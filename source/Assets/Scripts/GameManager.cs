using UnityEngine;
using System.Collections;

/// <summary>
/// Handles the game. Little task, right?
/// </summary>
public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Static instance of the class.
    /// </summary>
    public static GameManager Instance = null;

    /// <summary>
    /// Player prefab reference.
    /// </summary>
    public GameObject player;

    /// <summary>
    /// Time in seconds for showing level splash.
    /// </summary>
    public float levelStartDelay = 2f;

    /// <summary>
    /// Time in seconds for showing up the player on game start.
    /// </summary>
    public float playerSetupDelay = 1f;

    /// <summary>
    /// Lifes to be used when the game starts.
    /// </summary>
    public int initialLifes = 3;

    /// <summary>
    /// Clip to play when the player loses a life.
    /// </summary>
    public AudioClip loseLifeSound;

    /// <summary>
    /// State that represents when the player has started a new game.
    /// </summary>
    private bool _gameStarted = false;

    /// <summary>
    /// State that represents when the ball is moving.
    /// </summary>
    private bool _playing = false;

    /// <summary>
    /// Current player's lifes.
    /// </summary>
    private int _lifes = 0;

    /// <summary>
    /// Current level.
    /// </summary>
    private int _level = 0;

    /// <summary>
    /// Last level for play.
    /// </summary>
    private int _lastLevel = 9;

    /// <summary>
    /// To keep track of the quantity of bricks in the current level.
    /// </summary>
    private int _bricks = 0;

    /// <summary>
    /// A player clone representation.
    /// </summary>
    private GameObject _playerClone;

    protected void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    protected void Update()
    {
        if (Input.GetButtonDown("Cancel"))
        {
            if (MainMenu.Instance.IsVisible())
            {
                MainMenu.Instance.OnCancelAction();
            }
            else
            {
                MainMenu.Instance.Show();
            }
        }
    }

    protected void OnLevelWasLoaded(int index)
    {
        _level++;
        InitLevel();
    }

    /// <summary>
    /// Setup a new level.
    /// </summary>
    public void InitLevel()
    {
        ResumeGame();

        _playing = false;

        // UI updates.
        MainMenu.Instance.Hide();
        ModalDialog.Instance.Show("Level " + _level, levelStartDelay);
        InGameUI.Instance.UpdateCurrentLevel(_level);
        InGameUI.Instance.UpdateLifesQuantity(_lifes);
        InGameUI.Instance.Show();

        // Keep quantity of bricks in the current level.
        _bricks = GameObject.FindGameObjectsWithTag("Brick").Length;

        SetupPlayer();
    }

    /// <summary>
    /// Instantiates a player's prefab clone.
    /// </summary>
    private void SetupPlayer()
    {
        _playerClone = Instantiate(player, player.transform.position, Quaternion.identity) as GameObject;
    }

    /// <summary>
    /// Pauses the game. That was difficult, right?
    /// </summary>
    public void PauseGame()
    {
        Time.timeScale = 0;
    }

    /// <summary>
    /// Resumes the game.
    /// </summary>
    public void ResumeGame()
    {
        Time.timeScale = 1;
    }

    /// <summary>
    /// Resets the game and prepare for a new one.
    /// </summary>
    public void ResetGame()
    {
        _level = 0;
        _lifes = initialLifes;
        _gameStarted = true;
    }

    /// <summary>
    /// General check: lifes, game over, bricks.
    /// </summary>
    private void CheckStatus()
    {
        // Player is still alive?
        if (_lifes == 0)
        {
            GameOver();
        }

        // Player wins the current level.
        if (_bricks == 0)
        {
            // Player was playing the last level?
            if (_level == _lastLevel)
            {
                Win();
            }
            else
            {
                var nextLevel = _level + 1;
                Application.LoadLevel("Level" + nextLevel.ToString("00"));
            }
        }
    }

    /// <summary>
    /// Player loses the game. No more lifes!
    /// </summary>
    private void GameOver()
    {
        _gameStarted = false;

        ModalDialog.Instance.Show("Game Over", 2, () =>
        {
            MainMenu.Instance.Show();
        });
    }

    /// <summary>
    /// Player wins the game.
    /// </summary>
    private void Win()
    {
        _gameStarted = false;

        PauseGame();

        ModalDialog.Instance.Show("You WIN! You ROCK!");
    }

    /// <summary>
    /// Gets the current level being played.
    /// </summary>
    /// <returns>The current level.</returns>
    public int GetCurrentLevel()
    {
        return _level;
    }

    /// <summary>
    /// Discounts a life.
    /// </summary>
    public void LoseLife()
    {
        _lifes--;

        SoundManager.Instance.PlaySingle(loseLifeSound);

        InGameUI.Instance.UpdateLifesQuantity(_lifes);

        Destroy(_playerClone);

        Invoke("SetupPlayer", playerSetupDelay);

        CheckStatus();
    }

    /// <summary>
    /// Discounts a brick from the counter.
    /// </summary>
    public void DestroyBrick()
    {
        _bricks--;
        CheckStatus();
    }

    /// <summary>
    /// Verifies if the player has started a new game.
    /// </summary>
    /// <returns>If the player has started a new game or not.</returns>
    public bool IsGameStarted()
    {
        return _gameStarted;
    }

    /// <summary>
    /// Verifies if the player has launched the ball and still moving.
    /// </summary>
    /// <returns>If the player has launched the ball and still moving.</returns>
    public bool IsPlaying()
    {
        return _playing;
    }

    /// <summary>
    /// Sets playing state.
    /// </summary>
    /// <param name="value">If the player has launched the ball and still moving.</param>
    public void SetPlaying(bool value)
    {
        _playing = value;
    }
}
