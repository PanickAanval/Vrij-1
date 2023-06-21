using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Structure;
using Joeri.Tools.Utilities;

public partial class Player : Entity
{
    [Header("States:")]
    [SerializeField] private Dashing.Settings m_dashing;
    [SerializeField] private Falling.Settings m_falling;
    [SerializeField] private Grabbing.Settings m_grabbing;
    [SerializeField] private Idle.Settings m_idle;
    [SerializeField] private Jumping.Settings m_jumping;
    [SerializeField] private Launched.Settings m_launced;
    [SerializeField] private Throwing.Settings m_throwing;
    [SerializeField] private Walking.Settings m_walking;

    [Header("Player References:")]
    [SerializeField] private GrabbyHandler m_grabHandler;
    [Space]
    [SerializeField] private Transform m_center;

    //  Cache:
    private Controls.Results m_input;
    private Vector2 m_leftInputDir;

    //  TESTING PURPOSES:
    private IGrabbable m_grabbingItem = null;
    private bool m_airDashAvailable = true;

    #region Properties

    public Transform center                 { get => m_center; }

    public CharacterController controller   { get => m_movement.controller; }

    public Vector3 velocity                 { get => m_movement.velocity; set => m_movement.velocity = value; }
    public Vector2 flatVelocity             { get => m_movement.flatVelocity; set => m_movement.flatVelocity = value; }

    public float speed                      { get => m_movement.speed; set => m_movement.speed = value; }
    public float grip                       { get => m_movement.grip; set => m_movement.grip = value; }

    public PlayerController movement        { get => GetMovement<PlayerController>(); }

    public  bool onGround                   { get => m_movement.onGround; }
    public Vector3 lastGroundedPosition     { get => m_movement.lastGroundedPosition; }

    public float carrySmoothTime            { get => m_grabHandler.m_carrySmoothTime; }
    public Transform grabPivot              { get => m_grabHandler.grabPivot; }

    #endregion

    public void Setup()
    {
        m_grabHandler.Setup();
        m_movement = new PlayerController(gameObject, m_moveSettings);
        m_stateMachine = new FSM
            (
                typeof(Idle),
                new Idle(this, m_idle),
                new Walking(this, m_walking),
                new Falling(this, m_falling),
                new Jumping(this, m_jumping),
                new Grabbing(this, m_grabbing),
                new Throwing(this, m_throwing),
                new Launched(this, m_launced),
                new Dashing(this, m_dashing)
            );
    }

    public void Tick(Controls.Results input, float deltaTime, float cameraAngle)
    {
        //  We rotate the input vector by the angle of the camera, so that the player moves forward in relation to the camera at all times.
        m_input = input;
        m_leftInputDir = Vectors.RotateVector2(input.leftInput, cameraAngle);
        m_stateMachine.Tick(deltaTime);
    }

    public void Launch(float launchPower)
    {
        m_stateMachine.SwitchToState<Launched>().Setup(launchPower);
    }
}
