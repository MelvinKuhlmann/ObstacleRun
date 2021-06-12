using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionUI : MonoBehaviour
{
    public void ExitPause()
    {
        PlayerController.instance.Unpause();
    }

    public void RestartLevel()
    {
        ExitPause();
        SceneController.RestartZone();
    }
}