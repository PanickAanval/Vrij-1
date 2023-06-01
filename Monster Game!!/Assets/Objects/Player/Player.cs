using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Structure;
using Joeri.Tools.Utilities;

public partial class Player : MonoBehaviour
{
    [SerializeField] private PlayerController m_movement;
    [Space]
    [SerializeField] private Transform m_center;

    [Header("States:")]
    [SerializeField] private Falling.Settings m_falling;
    [SerializeField] private Jumping.Settings m_jumping;

    private FSM<Player> m_stateMachine = null;
    private Vector2 m_input;

    #region Properties

    public Transform center { get => m_center; }

    public Vector3 velocity { get => m_movement.velocity; }
    public Vector2 horizontalVelocity { get => m_movement.horizontalVelocity; }

    #endregion

    public void Setup()
    {
        m_stateMachine = new FSM<Player>(this, typeof(Walking), new Walking(), new Falling(m_falling), new Jumping(m_jumping));
    }

    public void Tick(Vector2 input, float deltaTime, float cameraAngle)
    {
        //  We rotate the input vector by the angle of the camera, so that the player moves forward in relation to the camera at all times.
        //  For now, the angle by which we rotate is negative. I have yet to find out why it rotates the wrong way.
        m_input = Vectors.RotateVector2(input, cameraAngle);
        m_stateMachine.Tick(deltaTime);
    }

    public void DrawGizmos()
    {
        m_stateMachine.DrawGizmos(transform.position + (Vector3.up * (m_movement.controller.height + m_movement.controller.radius)));
        m_movement.DrawGizmos();
    }
}
