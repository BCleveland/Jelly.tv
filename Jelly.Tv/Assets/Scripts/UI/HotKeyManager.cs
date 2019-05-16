using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class HotKeyManager : MonoBehaviour
{
    [SerializeField]
    private UIButton m_pauseButton = null;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            print("pause hotkey pressed");
            m_pauseButton.ExecuteClick();
        }
    }
}
