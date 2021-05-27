using UnityEngine;

public class AreaEntrance : MonoBehaviour
{
    public string transitionName;

    private void Start()
    {
        if (transitionName.Equals(GameManager.Instance.areaTransitionName))
        {
            PlayerController.instance.transform.position = transform.position;
        }
    }
}
