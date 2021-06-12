using UnityEngine;

public class LocalTransformDuplicator : MonoBehaviour
{
    public Transform targetTrasnform;

    private void OnEnable()
    {
        transform.localScale = targetTrasnform.localScale;
        transform.localRotation = targetTrasnform.localRotation;
        transform.localPosition = targetTrasnform.localPosition;
    }
}
