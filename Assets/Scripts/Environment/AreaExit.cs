using UnityEngine;

public class AreaExit : MonoBehaviour
{
    public string sceneToLoad = string.Empty;
    public string areaTransitionName = string.Empty;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.Player.ToString()))
        {
            GameManager.Instance.areaTransitionName = areaTransitionName;
            LevelManager.Instance.GoToNextLevel(sceneToLoad);
        }
    }
}
