using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Movement;

public partial class SpringyFella : Monster, IStandable
{
    [Header("Springy Fella Properties:")]
    [SerializeField] private float m_launchPower = 20f;
    [Space]
    [SerializeField] private float m_detectionRange = 5f;
    [SerializeField] private float m_lookAheadTime = 0.1f;
    [Space]
    [SerializeField] private float m_stunnedTime = 0.5f;

    //  Run-time:
    private FSM m_stateMachine = null;

    public override void Setup(Player player)
    {
        base.Setup(player);
        m_stateMachine = new FSM
            (
                typeof(Idle),
                new Falling(this, typeof(Stunned)),
                new PickedUp(this),
                new Thrown(this),
                new Idle(this),
                new Follow(this),
                new Stunned(this)
            );
    }

    public override void Tick(float deltaTime)
    {
        m_stateMachine.Tick(deltaTime);
    }

    public void OnStand(Entity entity)
    {
        if (entity.GetType() != typeof(Player)) return;

        var player = entity as Player;

        player.Launch(m_launchPower);
    }

    public override void OnGrab(Player player)
    {
        m_stateMachine.SwitchToState<PickedUp>().Setup(player.carrySmoothTime, player.grabber);
    }

    public override void OnRelease(Player player, Vector3 releaseVelocity)
    {
        m_stateMachine.SwitchToState<Thrown>().Setup(releaseVelocity);
    }

    public override void DrawGizmos()
    {
        m_stateMachine.DrawGizmos(transform.position);
        base.DrawGizmos();
    }
}

