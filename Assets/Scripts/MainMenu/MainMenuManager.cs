using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
	public void StartGame()
	{
		//TODO set specific scenename here
		Debug.Log("Start Game");
		SceneManager.LoadScene(1);
	}

	public void LoadGame()
	{
		//TODO get last visited scene from playerpreferences
		Debug.Log("Load Game");
		SceneManager.LoadScene(1);
	}

	public void Credits()
    {
		//TODO show credits
		Debug.Log("Credits");
	}

	public void QuitGame()
	{
		Debug.Log("Quit!");
		Application.Quit();
	}
}
