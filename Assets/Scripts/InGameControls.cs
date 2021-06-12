using UnityEngine;

public class InGameControls : MonoBehaviour
{
   // private bool menuIsActive = false;
  //  public GameObject inGameMenu;

    public float doubleTapTime = 0.25f;
    private float lastTapTimeLeft;
    private float lastTapTimeRight;

    private void Awake()
    {
    //    inGameMenu.SetActive(menuIsActive);
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerController.instance.HandleJump();
        }

        if (Input.GetKey(KeyCode.Space))
        {
            PlayerController.instance.HandleKeepJumping();
        }

        if (Input.GetKeyUp(KeyCode.Space))
        {
            PlayerController.instance.HandleStopJump();
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
                PlayerController.instance.HandleRightDash();
            }
            lastTapTimeRight = Time.time;
        }

        // double tap check
        if (Input.GetKeyDown(KeyCode.LeftArrow)) // TODO fix double tap, not always triggers correctly
        {
            float timeSinceLastTap = Time.time - lastTapTimeLeft;

            if (timeSinceLastTap <= doubleTapTime)
            {
                PlayerController.instance.HandleLeftDash();
            }
            lastTapTimeLeft = Time.time;
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            PlayerController.instance.HandleRightMovement();
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            PlayerController.instance.HandleLeftMovement();
        } else
        {
            PlayerController.instance.HandleIdle();
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            PlayerController.instance.HandleAttack();
        }
    }
}
