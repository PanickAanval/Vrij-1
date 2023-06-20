using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Structure;
using Joeri.Tools.Utilities;

public partial class Player : Entity
{
    [Header("States:")]
    [SerializeField] private Grabbing.Settings m_grabbing;
    [SerializeField] private Throwing.Settings m_throwing;
    [SerializeField] private Dashing.Settings m_dashing;
    [Space]
    [SerializeField] private AnimationSettings m_animations;

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

    public float carrySmoothTime            { get => m_grabHandler.m_carrySmoothTime; }
    public Transform grabPivot              { get => m_grabHandler.grabPivot; }

    #endregion

    public void Setup()
    {
        m_grabHandler.Setup();
        m_movement = new PlayerController(gameObject, m_moveSettings);
        m_stateMachine = new FSM
            (
                typeof(Walking),
                new Walking(this),
                new Falling(this),
                new Jumping(this),
                new Grabbing(this, m_grabbing),
                new Throwing(this, m_throwing),
                new Launched(this),
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

    [System.Serializable]
    public class AnimationSettings
    {
        public AnimationClip idle;
        [Space]
        public AnimationClip startJump;
        public AnimationClip falling;
        public AnimationClip jumpLand;
        [Space]
        public AnimationClip startRun;
        public AnimationClip endRun;
        [Space]
        public AnimationClip dashing;
        [Space]
        public AnimationClip grabbing;
        public AnimationClip throwing;
    }
}
