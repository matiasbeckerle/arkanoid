using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// To handle panels toggling, etc.
    /// </summary>
    public enum MenuPanel { Main, About };



    /// <summary>
    /// Static instance of the class.
    /// </summary>
    public static MainMenu Instance = null;

    /// <summary>
    /// The play button reference.
    /// </summary>
    public GameObject playButton;

    /// <summary>
    /// The play button text reference.
    /// </summary>
    public Text playButtonText;

    /// <summary>
    /// The continue button reference.
    /// </summary>
    public GameObject continueButton;

    /// <summary>
    /// Reference for the "main" panel.
    /// </summary>
    public GameObject panelMain;

    /// <summary>
    /// Reference for the "about" panel.
    /// </summary>
    public GameObject panelAbout;

    /// <summary>
    /// Panel being used.
    /// </summary>
    private MenuPanel _currentPanel;

    /// <summary>
    /// Previous panel for back functionality.
    /// </summary>
    private MenuPanel _previousPanel;

    /// <summary>
    /// Flag for knowing when the main menu is visible or not.
    /// </summary>
    private bool _isVisible;

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

    protected void Start()
    {
        if (GameManager.Instance.IsGameStarted())
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    /// <summary>
    /// Sets the default button as selected.
    /// </summary>
    /// <param name="defaultButton">The button to be set as selected.</param>
    private void SetDefaultButton(GameObject defaultButton)
    {
        GameObject myEventSystem = GameObject.Find("EventSystem");
        myEventSystem.GetComponent<UnityEngine.EventSystems.EventSystem>().SetSelectedGameObject(defaultButton);
    }

    /// <summary>
    /// Shows the MainMenu in the scene.
    /// </summary>
    public void Show()
    {
        GameManager.Instance.PauseGame();
        SoundManager.Instance.PauseMusic();
        SoundManager.Instance.PlayMainMenuTrack();

        _isVisible = true;
        gameObject.SetActive(true);

        CheckButtonsState();
    }

    /// <summary>
    /// Based on current game status, sets visibility and states for buttons.
    /// </summary>
    private void CheckButtonsState()
    {
        if(GameManager.Instance.IsGameStarted())
        {
            playButtonText.text = "New game";
            continueButton.SetActive(true);
            SetDefaultButton(continueButton);
        }
        else
        {
            playButtonText.text = "Play!";
            continueButton.SetActive(false);
            SetDefaultButton(playButton);
        }
    }

    public void OnCancelAction()
    {
        if (_currentPanel != MenuPanel.Main)
        {
            Back();
        }
        else if (GameManager.Instance.IsGameStarted())
        {
            Hide();
        }
    }

    /// <summary>
    /// Hides the MainMenu from the scene.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
        _isVisible = false;

        SoundManager.Instance.PauseMainMenuTrack();
        SoundManager.Instance.PlayMusic();
        GameManager.Instance.ResumeGame();
    }

    /// <summary>
    /// Toggles visibility from a given panel and hides the others.
    /// </summary>
    void LoadPanel(MenuPanel panel = MenuPanel.Main)
    {
        // Save the previous panel.
        _previousPanel = _currentPanel;

        // Set current panel.
        _currentPanel = panel;

        // Hide all panels.
        HidePanels();

        switch (_currentPanel)
        {
            case MenuPanel.About:
                panelAbout.SetActive(true);
                break;

            default:
                panelMain.SetActive(true);
                break;
        }
    }

    /// <summary>
    /// Sets visibility to false for all panels.
    /// </summary>
    private void HidePanels()
    {
        panelMain.SetActive(false);
        panelAbout.SetActive(false);
    }

    /// <summary>
    /// Loads the previous panel.
    /// </summary>
    private void Back()
    {
        LoadPanel(_previousPanel);
    }

    /// <summary>
    /// Gets if the main menu is visible or not.
    /// </summary>
    /// <returns>Main menu's visibility status.</returns>
    public bool IsVisible()
    {
        return _isVisible;
    }

    /// <summary>
    /// Callback for "play" button.
    /// Starts a new game.
    /// </summary>
    public void OnPlayButtonClick()
    {
        GameManager.Instance.ResetGame();
        Application.LoadLevel("Level01");
    }

    /// <summary>
    /// Callback for "continue" button.
    /// Closes the MainMenu in order to continue playing.
    /// </summary>
    public void OnContinueButtonClick()
    {
        Hide();
    }

    /// <summary>
    /// Callback for "about" button.
    /// Opens "About" panel.
    /// </summary>
    public void OnAboutButtonClick()
    {
        LoadPanel(MenuPanel.About);
    }

    /// <summary>
    /// Callback for "exit" button.
    /// Closes the game.
    /// </summary>
    public void OnExitButtonClick()
    {
        Application.Quit();
    }
}
