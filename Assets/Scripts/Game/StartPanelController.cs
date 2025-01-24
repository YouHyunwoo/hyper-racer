using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPanelController : MonoBehaviour
{
    public delegate void OnClickStartButtonDelegate();
    public event OnClickStartButtonDelegate OnClickStartButtonEvent;

    public void OnClickStartButton()
    {
        OnClickStartButtonEvent?.Invoke();
    }
}
