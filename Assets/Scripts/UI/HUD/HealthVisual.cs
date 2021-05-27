using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

// https://www.youtube.com/watch?v=xWCJfE_sAXE : 23:00
public class HealthVisual : MonoBehaviour
{
    [SerializeField]
    private Sprite health0Sprite;
    [SerializeField]
    private Sprite health1Sprite;
    [SerializeField]
    private Sprite health2Sprite;
    [SerializeField]
    private Sprite health3Sprite;
    [SerializeField]
    private Sprite health4Sprite;

    private List<HealthImage> healthImageList;
    private HealthSystem healthSystem;

    private void Awake()
    {
        healthImageList = new List<HealthImage>();
    }

    private void Start()
    {
        HealthSystem healthSystem = new HealthSystem(4);
        SetHealthSystem(healthSystem);
    }

    public void SetHealthSystem(HealthSystem healthSystem)
    {
        this.healthSystem = healthSystem;

        List<HealthSystem.Health> healthList = healthSystem.GetHealthList();
        Vector2 healthAnchoredPosition = new Vector2(0, 0);
        for (int i = 0; i < healthList.Count; i++)
        {
            HealthSystem.Health health = healthList[i];
            CreateHealthImage(healthAnchoredPosition).SetHealthFragments(health.GetFragmentAmount());
            healthAnchoredPosition += new Vector2(40, 0);
        }

        healthSystem.OnDamaged += HealthSystem_OnDamaged;
        healthSystem.OnHealed += HealthSystem_OnHealed;
    }

    private void HealthSystem_OnDamaged(object sender, System.EventArgs e)
    {
        // Health system was damaged
        RefreshAllHealth();
    }

    private void HealthSystem_OnHealed(object sender, System.EventArgs e)
    {
        // Health system was healed
        RefreshAllHealth();
    }

    private void RefreshAllHealth()
    {
        List<HealthSystem.Health> healtList = healthSystem.GetHealthList();
        for (int i = 0; i < healtList.Count; i++)
        {
            HealthImage healthImage = healthImageList[i];
            HealthSystem.Health health = healtList[i];
            healthImage.SetHealthFragments(health.GetFragmentAmount());
        }
    }

    private HealthImage CreateHealthImage(Vector2 anchoredPosition)
    {
        GameObject healthGameObject = new GameObject("Health", typeof(Image));
        // Set as child of this transform
        // heartGameObject.transform.parent = transform;
        healthGameObject.transform.SetParent(transform);
        healthGameObject.transform.localPosition = Vector3.zero;

        // Locate and Size of object
        healthGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;
        healthGameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(40, 40);

        // Set heart sprite
        Image heartImageUI = healthGameObject.GetComponent<Image>();
        heartImageUI.sprite = health0Sprite;

        HealthImage heartImage = new HealthImage(this, heartImageUI);
        healthImageList.Add(heartImage);
        return heartImage;
    }

    // Represents a single heart
    public class HealthImage
    {
        private HealthVisual healthVisual;
        private Image healthImage;

        public HealthImage(HealthVisual healthVisual, Image healthImage)
        {
            this.healthVisual = healthVisual;
            this.healthImage = healthImage;
        }

        public void SetHealthFragments(int fragments)
        {
            switch (fragments)
            {
                case 0:
                    healthImage.sprite = healthVisual.health0Sprite;
                    break;
                case 1:
                    healthImage.sprite = healthVisual.health1Sprite;
                    break;
                case 2:
                    healthImage.sprite = healthVisual.health2Sprite;
                    break;
                case 3:
                    healthImage.sprite = healthVisual.health3Sprite;
                    break;
                case 4:
                    healthImage.sprite = healthVisual.health4Sprite;
                    break;
            }
        }
    }
}
