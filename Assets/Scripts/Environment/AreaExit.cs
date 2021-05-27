using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaExit : MonoBehaviour
{
    public string sceneToLoad = string.Empty;

    public string areaTransitionName = string.Empty;

    /*public float waitToLoad = 1f;
    private bool shouldLoadAfterFade;
*/
   /* private void Start()
    {
        if (SceneTransition.instance != null)
        {
            SceneTransition.instance.FadeFromBlack();
        }
    }*/

  /*  private void Update()
    {
        if (shouldLoadAfterFade)
        {
            waitToLoad -= Time.deltaTime;
            if (waitToLoad <= 0)
            {
                shouldLoadAfterFade = false;
                SceneManager.LoadScene(sceneToLoad);
            }
        }
    }*/

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player.ToString()))
        {
           // shouldLoadAfterFade = true;
          //  SceneTransition.instance.FadeToBlack();
            GameManager.Instance.areaTransitionName = areaTransitionName;
            LevelManager.instance.GoToNextLevel(sceneToLoad);
        }
    }
}
