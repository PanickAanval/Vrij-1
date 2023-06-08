using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Structure;
using Joeri.Tools.Utilities;

public partial class Player : Entity
{
    [Header("Player Properties:")]
    [SerializeField] private float m_grabTime = 1f;
    [SerializeField] private float m_grabGrip = 1f;
    [Space]
    [SerializeField] private float m_carrySmoothTime = 1f;
    [SerializeField] private float m_throwStrength = 5f;

    [Header("States:")]
    [SerializeField] private Grabbing.Settings m_grabbing;
    [SerializeField] private Throwing.Settings m_throwing;

    [Header("Player References:")]
    [SerializeField] private GrabbyHandler m_grabHandler;
    [Space]
    [SerializeField] private Transform m_center;
    [SerializeField] private Transform m_grabPivot;

    //  Cache:
    private Vector2 m_input;

    //  TESTING PURPOSES:
    private IGrabbable m_grabbingItem = null;
    private bool m_airJumpAvailable = true;

    #region Properties

    public float carrySmoothTime { get => m_carrySmoothTime; }

    public Transform center { get => m_center; }
    public Transform grabPivot { get => m_grabPivot; }

    public Vector3 velocity { get => m_movement.velocity; }
    public Vector2 flatVelocity { get => m_movement.flatVelocity; }

    public float speed { get => m_movement.speed; set => m_movement.speed = value; }
    public float grip { get => m_movement.grip; set => m_movement.grip = value; }

    public PlayerController movement { get => GetMovement<PlayerController>(); }

    #endregion

    public void Setup()
    {
        m_grabHandler.Setup(this);
        m_movement = new PlayerController(gameObject, m_moveSettings);
        m_stateMachine = new FSM
            (
                typeof(Walking),
                new Walking(this),
                new Falling(this),
                new Jumping(this),
                new Grabbing(this, m_grabbing),
                new Throwing(this, m_throwing),
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
}
