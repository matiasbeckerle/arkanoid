using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class InGameUI : MonoBehaviour
{
    public static InGameUI Instance = null;
    public Text currentLevelText;
    public Text lifesQuantityText;

    void Awake()
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

    public void UpdateLifesQuantity(int lifesQuantity)
    {
        lifesQuantityText.text = lifesQuantity.ToString();
    }

    public void UpdateCurrentLevel(int level)
    {
        currentLevelText.text = level.ToString();
    }
}
