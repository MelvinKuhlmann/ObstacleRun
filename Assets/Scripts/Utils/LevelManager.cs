using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region Singleton
    public static LevelManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            return;
        }

        instance = this;
    }
    #endregion

    public Image fadeImage;
    public Animator anim;

    public void GoToNextLevel(string sceneName)
    {
        SaveSystem.SavePlayer(PlayerController.instance);
        StartCoroutine(Fading(sceneName));
    }

    IEnumerator Fading(string sceneName)
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        SceneManager.LoadScene(sceneName);
    }
}
