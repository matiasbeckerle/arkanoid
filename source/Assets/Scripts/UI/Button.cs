using UnityEngine;

public class Button : MonoBehaviour
{
    /// <summary>
    /// Sound to be played on select.
    /// </summary>
    public AudioClip selectSound;

    /// <summary>
    /// Sound to be played on enter.
    /// </summary>
    public AudioClip enterSound;

    public void OnSelect()
    {
        SoundManager.Instance.PlaySingle(selectSound);
    }

    public void OnSubmit()
    {
        SoundManager.Instance.PlaySingle(enterSound);
    }
}
