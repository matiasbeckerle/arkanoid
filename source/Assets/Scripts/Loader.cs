using UnityEngine;
using System.Reflection;

[assembly: AssemblyVersion("0.1.2.*")]
public class Loader : MonoBehaviour
{
    /// <summary>
    /// DebugUI prefab to instantiate.
    /// </summary>
    [SerializeField]
    private GameObject _debugUI;

    /// <summary>
    /// ModalDialog prefab to instantiate.
    /// </summary>
    [SerializeField]
    private GameObject _modalDialog;

    /// <summary>
    /// MainMenu prefab to instantiate.
    /// </summary>
    [SerializeField]
    private GameObject _mainMenu;

    /// <summary>
    /// InGameUI prefab to instantiate.
    /// </summary>
    [SerializeField]
    private GameObject _inGameUI;

    /// <summary>
    /// SoundManager prefab to instantiate.
    /// </summary>
    [SerializeField]
    private GameObject _soundManager;

    /// <summary>
    /// GameManager prefab to instantiate.
    /// </summary>
    [SerializeField]
    private GameObject _gameManager;

    protected void Awake()
    {
        // Check if the instances have already been assigned to static variables or they are still null.

        if (DebugUI.Instance == null)
        {
            Instantiate(_debugUI);
        }
        if (ModalDialog.Instance == null)
        {
            Instantiate(_modalDialog);
        }
        if (MainMenu.Instance == null)
        {
            Instantiate(_mainMenu);
        }
        if (InGameUI.Instance == null)
        {
            Instantiate(_inGameUI);
        }
        if (SoundManager.Instance == null)
        {
            Instantiate(_soundManager);
        }
        if (GameManager.Instance == null)
        {
            Instantiate(_gameManager);
        }
    }
}
