using UnityEngine;
using System.Collections;

public class LoadOnClick2 : MonoBehaviour
{

    public void LoadScene(int level)
    {
        Application.LoadLevel(level);
    }
}