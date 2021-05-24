using UnityEngine;

public class InGameMenuManager : MonoBehaviour
{
    private bool menuIsActive = false;
    public GameObject inGameMenu;

    private void Start()
    {
        inGameMenu.SetActive(menuIsActive);
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menuIsActive = !menuIsActive;
            GameManager.Instance.Pause(menuIsActive);
            inGameMenu.SetActive(menuIsActive);
        }
    }
}
