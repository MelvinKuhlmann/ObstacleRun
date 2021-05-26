using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public Image fadeImage;
    public Animator anim;


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player.ToString()))
        {
            StartCoroutine(Fading());
        }
    }

    IEnumerator Fading()
    {
        anim.SetBool("Fade", true);
        yield return new WaitUntil(() => fadeImage.color.a == 1);
        SceneManager.LoadScene(2);
    }
}
