using UnityEngine;
using System.Collections;

public class BringToFront : MonoBehaviour
{
    protected void OnEnable()
    {
        transform.SetAsLastSibling();
    }
}
