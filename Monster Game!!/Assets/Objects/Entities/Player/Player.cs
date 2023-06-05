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
    [SerializeField] private float m_throwStrength = 5f;

    [Header("Player References:")]
    [SerializeField] private PlayerController m_movement;
    [SerializeField] private GrabbyHandler m_grabbing;
    [Space]
    [SerializeField] private Transform m_center;
    [SerializeField] private Transform m_grabber;

    //  Run-time:
    private FSM m_stateMachine = null;

    //  Cache:
    private Vector2 m_input;

    //  TESTING PURPOSES:
    private IGrabbable m_grabbingItem = null;
    private bool m_airJumpAvailable = true;

    #region Properties

    public float carrySmoothTime { get => m_carrySmoothTime; }

    public Transform center { get => m_center; }
    public Transform grabber { get => m_grabber; }

    public CharacterController controller { get => m_movement.controller; }

    public Vector3 velocity { get => m_movement.velocity; }
    public Vector2 flatVelocity { get => m_movement.flatVelocity; }

    public float speed { get => m_movement.speed; set => m_movement.speed = value; }
    public float grip { get => m_movement.grip; set => m_movement.grip = value; }

    #endregion

    public void Setup()
    {
        m_grabbing.Setup(this);
        m_stateMachine = new FSM
            (
                typeof(Walking),
                new Walking(this),
                new Falling(this),
                new Jumping(this),
                new Grabbing(this),
                new Throwing(this),
                new Launched(this)
            );
    }

    public void Tick(Vector2 input, float deltaTime, float cameraAngle)
    {
        //  We rotate the input vector by the angle of the camera, so that the player moves forward in relation to the camera at all times.
        //  For now, the angle by which we rotate is negative. I have yet to find out why it rotates the wrong way.
        m_input = Vectors.RotateVector2(input, cameraAngle);
        m_stateMachine.Tick(deltaTime);
    }

    public void Launch(float launchPower)
    {
        m_stateMachine.SwitchToState<Launched>().Setup(launchPower);
    }

    public override void DrawGizmos()
    {
        m_stateMachine.DrawGizmos(transform.position + (Vector3.up * (m_movement.controller.height + m_movement.controller.radius)));
        m_movement.DrawGizmos();
    }
}
