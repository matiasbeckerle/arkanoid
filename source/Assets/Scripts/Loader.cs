using UnityEngine;
using System.Reflection;

[assembly: AssemblyVersion("0.1.2.*")]
public class Loader : MonoBehaviour
{
    // DebugUI prefab to instantiate.
    [SerializeField]
    GameObject debugUI;

    // ModalDialog prefab to instantiate.
    [SerializeField]
    GameObject modalDialog;

    // MainMenu prefab to instantiate.
    [SerializeField]
    GameObject mainMenu;

    // InGameUI prefab to instantiate.
    [SerializeField]
    GameObject inGameUI;

    // SoundManager prefab to instantiate.
    [SerializeField]
    GameObject soundManager;

    // GameManager prefab to instantiate.
    [SerializeField]
    GameObject gameManager;

    void Awake()
    {
        // Check if the instances have already been assigned to static variables or they are still null.

        if (DebugUI.Instance == null)
        {
            Instantiate(debugUI);
        }
        if (ModalDialog.Instance == null)
        {
            Instantiate(modalDialog);
        }
        if (MainMenu.Instance == null)
        {
            Instantiate(mainMenu);
        }
        if (InGameUI.Instance == null)
        {
            Instantiate(inGameUI);
        }
        if (SoundManager.Instance == null)
        {
            Instantiate(soundManager);
        }
        if (GameManager.Instance == null)
        {
            Instantiate(gameManager);
        }
    }
}
