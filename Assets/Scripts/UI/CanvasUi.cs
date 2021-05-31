using UnityEngine;

public class CanvasUi : MonoBehaviour
{
    #region singleton
    private static CanvasUi instance;
    public static CanvasUi Instance { get { return instance; } }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion
}
