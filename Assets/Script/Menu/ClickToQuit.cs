using UnityEngine;
using System.Collections;

public class ClickToQuit : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKey("escape"))
            Application.Quit();

    }

    public void doExitGame()
    {
        Application.Quit();
    }
}