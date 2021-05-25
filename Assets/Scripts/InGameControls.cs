using UnityEngine;

public class InGameControls : MonoBehaviour
{
    private PlayerController playerController;

    private bool menuIsActive = false;
    public GameObject inGameMenu;

    public float doubleTapTime = 0.25f;
    private float lastTapTimeLeft;
    private float lastTapTimeRight;

    private void Awake()
    {
        playerController = FindObjectOfType<PlayerController>();
        inGameMenu.SetActive(menuIsActive);
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void Update()
    {
        HandleMenuStuff();
    }

    private void HandleMovement()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            playerController.HandleJump();
        }

        if (Input.GetKey(KeyCode.RightArrow) && Input.GetKey(KeyCode.LeftArrow))
        {
            return;
        }

        // double tap check
        if (Input.GetKeyDown(KeyCode.RightArrow)) // TODO fix double tap, not always triggers correctly
        {
            float timeSinceLastTap = Time.time - lastTapTimeRight;

            if (timeSinceLastTap <= doubleTapTime)
            {
                playerController.HandleRightDash();
            }
            lastTapTimeRight = Time.time;
        }

        // double tap check
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // TODO fix double tap, not always triggers correctly
        {
            float timeSinceLastTap = Time.time - lastTapTimeLeft;

            if (timeSinceLastTap <= doubleTapTime)
            {
                playerController.HandleLeftDash();
            }
            lastTapTimeLeft = Time.time;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            playerController.HandleRightMovement();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            playerController.HandleLeftMovement();
        } else
        {
            playerController.HandleIdle();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            playerController.HandleAttack();
        }
    }

    private void HandleMenuStuff()
    {
        // Needs to be in the Update and not FixedUpdate because Game ScaleTime is set to 0 when paused, FixedUpdate is not triggered at that time
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            menuIsActive = !menuIsActive;
            GameManager.Instance.Pause(menuIsActive);
            inGameMenu.SetActive(menuIsActive);
        }
    }
}
