using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class CharacterController2D : MonoBehaviour
{
    [Tooltip("The Layers which represent gameobjects that the Character Controller can be grounded on.")]
    public LayerMask groundedLayerMask;
    public Transform feetPos;
    public float checkRadius;
    public Transform frontCheck;

    Rigidbody2D m_Rigidbody2D;
    Vector2 m_PreviousPosition;
    Vector2 m_CurrentPosition;
    Vector2 m_NextMovement;

    public bool IsGrounded { get; protected set; }
    public bool IsCeilinged { get; protected set; }
    public Vector2 Velocity { get; protected set; }
    public Rigidbody2D Rigidbody2D { get { return m_Rigidbody2D; } }


    void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        m_CurrentPosition = m_Rigidbody2D.position;
        m_PreviousPosition = m_Rigidbody2D.position;

        Physics2D.queriesStartInColliders = false;
    }

    void FixedUpdate()
    {
        m_PreviousPosition = m_Rigidbody2D.position;
        m_CurrentPosition = m_PreviousPosition + m_NextMovement;
        Velocity = (m_CurrentPosition - m_PreviousPosition) / Time.deltaTime;

        m_Rigidbody2D.MovePosition(m_CurrentPosition);
        m_NextMovement = Vector2.zero;

        CheckCapsuleEndCollisions();
    }

    /// <summary>
    /// This moves a rigidbody and so should only be called from FixedUpdate or other Physics messages.
    /// </summary>
    /// <param name="movement">The amount moved in global coordinates relative to the rigidbody2D's position.</param>
    public void Move(Vector2 movement)
    {
        m_NextMovement += movement;
    }

    /// <summary>
    /// This moves the character without any implied velocity.
    /// </summary>
    /// <param name="position">The new position of the character in global space.</param>
    public void Teleport(Vector2 position)
    {
        Vector2 delta = position - m_CurrentPosition;
        m_PreviousPosition += delta;
        m_CurrentPosition = position;
        m_Rigidbody2D.MovePosition(position);
    }

    /// <summary>
    /// This updates the state of IsGrounded.  It is called automatically in FixedUpdate but can be called more frequently if higher accurracy is required.
    /// </summary>
    public void CheckCapsuleEndCollisions()
    {
        IsGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, groundedLayerMask);
    }
}