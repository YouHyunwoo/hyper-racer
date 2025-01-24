using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void MoveButtonDelegate();

public class MoveButton : MonoBehaviour
{
    bool isDown;
    public event MoveButtonDelegate OnMoveButtonDown;

    public void ButtonDown()
    {
        isDown = true;
    }

    public void ButtonUp()
    {
        isDown = false;
    }

    void Update()
    {
        if (isDown)
        {
            OnMoveButtonDown?.Invoke();
        }
    }
}
