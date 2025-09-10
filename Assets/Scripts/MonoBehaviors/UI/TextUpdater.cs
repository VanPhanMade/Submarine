using System;
using TMPro;
using UnityEngine;

public class TextUpdater : MonoBehaviour
{
    public Action Enabled;
    private void OnEnable()
    {
        Enabled?.Invoke();
    }


}
