using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Structure;
using Joeri.Tools.Utilities;

public partial class Player : Entity
{
    [Header("Player Properties:")]
    [SerializeField] private float m_jumpForce = 5f;
    [Space]
    [SerializeField] private float m_grabTime = 1f;
    [SerializeField] private float m_grabGrip = 1f;
    [Space]
    [SerializeField] private float m_carrySmoothTime = 1f;

    [Header("Player References:")]
    [SerializeField] private PlayerController m_movement;
    [SerializeField] private GrabbyHandler m_grabbing;
    [Space]
    [SerializeField] private Transform m_center;
    [SerializeField] private Transform m_grabber;

    //  Run-time:
    private FSM<Player> m_stateMachine = null;

    //  Cache:
    private Vector2 m_input;

    #region Properties

    public float carrySmoothTime { get => m_carrySmoothTime; }

    public Transform center { get => m_center; }
    public Transform grabber { get => m_grabber; }

    public Vector3 velocity { get => m_movement.velocity; }
    public Vector2 flatVelocity { get => m_movement.flatVelocity; }

    #endregion

    public void Setup()
    {
        m_stateMachine = new FSM<Player>(this, typeof(Walking), new Walking(), new Falling(), new Jumping(), new Grabbing(m_grabTime));
        m_grabbing.Setup(this);
    }

    public void Tick(Vector2 input, float deltaTime, float cameraAngle)
    {
        //  We rotate the input vector by the angle of the camera, so that the player moves forward in relation to the camera at all times.
        //  For now, the angle by which we rotate is negative. I have yet to find out why it rotates the wrong way.
        m_input = Vectors.RotateVector2(input, cameraAngle);
        m_stateMachine.Tick(deltaTime);
    }

    public State SetGuestState<State>(State state) where State : State<Player>
    {
        return m_stateMachine.SwitchToGuestState(state);
    }

    public override void DrawGizmos()
    {
        m_stateMachine.DrawGizmos(transform.position + (Vector3.up * (m_movement.controller.height + m_movement.controller.radius)));
        m_movement.DrawGizmos();
    }
}
