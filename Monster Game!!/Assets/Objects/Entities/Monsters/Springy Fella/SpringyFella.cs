using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Movement;

public partial class SpringyFella : Monster
{
    [Header("Springy Fella Properties:")]
    [SerializeField] private float m_detectionRange = 5f;
    [SerializeField] private float m_lookAheadTime = 0.1f;
    [Space]
    [SerializeField] private float m_stunnedTime = 0.5f;

    [Header("Springy Fella States:")]
    [SerializeField] protected Idle.Settings m_idle;
    [SerializeField] protected Follow.Settings m_follow;
    [SerializeField] protected Launching.Settings m_launching;
    [SerializeField] protected Stunned.Settings m_stunned;

    [Header("Springy Fella References:")]
    [SerializeField] private PlayerLauncher m_launcher;

    public override void Setup(Player player)
    {
        base.Setup(player);
        m_stateMachine = new FSM
            (
                typeof(Idle),
                new Idle(this, m_idle),
                new Follow(this, m_follow),
                new Launching(this, m_launching),
                new Falling(this, m_falling ,typeof(Stunned)),
                new PickedUp(this, m_pickedUp),
                new Thrown(this, m_thrown),
                new Stunned(this, m_stunned)
            );

        m_launcher.onLaunch += OnLaunchPlayer;
    }

    private void OnLaunchPlayer()
    {
        m_stateMachine.SwitchToState(typeof(Launching));
    }
}

