using UnityEngine;

public class GameManager : MonoBehaviour
{
    private int doorsDestroyed = 0;

    #region Singleton
    public static GameManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }
    #endregion


    public void AddDoorsDestroyed()
    {
        doorsDestroyed++;
        Debug.Log("Doors destroyed total: " + doorsDestroyed);
    }

}
