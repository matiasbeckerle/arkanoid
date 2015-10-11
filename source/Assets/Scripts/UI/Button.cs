using UnityEngine;

public class Button : MonoBehaviour
{
    public AudioClip selectSound;
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
