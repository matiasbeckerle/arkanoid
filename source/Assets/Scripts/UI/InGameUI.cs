using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUI : MonoBehaviour
{
    /// <summary>
    /// Static instance of the class.
    /// </summary>
    public static InGameUI Instance = null;

    /// <summary>
    /// Current level text reference.
    /// </summary>
    public Text currentLevelText;

    /// <summary>
    /// Quantity of lifes text reference.
    /// </summary>
    public Text lifesQuantityText;

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
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Shows the InGameUI in the scene.
    /// </summary>
    public void Show()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the InGameUI from the scene.
    /// </summary>
    public void Hide()
    {
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Updates the lifes quantity on the UI.
    /// </summary>
    /// <param name="lifesQuantity">The quantity of lifes left.</param>
    public void UpdateLifesQuantity(int lifesQuantity)
    {
        lifesQuantityText.text = lifesQuantity.ToString();
    }

    /// <summary>
    /// Updates the current level on the UI.
    /// </summary>
    /// <param name="level">The level to be shown.</param>
    public void UpdateCurrentLevel(int level)
    {
        currentLevelText.text = level.ToString();
    }
}
