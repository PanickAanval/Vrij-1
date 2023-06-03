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

    //  Run-time:
    private FSM<SpringyFella> m_stateMachine = null;

    public override void Setup(Player player)
    {
        base.Setup(player);
        m_stateMachine = new FSM<SpringyFella>(this, typeof(Idle), new Idle(), new Follow(), new Falling());
    }

    public override void TickSubclass(float deltaTime)
    {
        m_stateMachine.Tick(deltaTime);
    }

    public void OnStand(Entity entity)
    {
        if (entity.GetType() != typeof(Player)) return;

        var player = entity as Player;

        player.SetGuestState(new Player.Launched()).Setup(m_launchPower);
    }

    public override void DrawGizmos()
    {
        m_stateMachine.DrawGizmos(transform.position);
        base.DrawGizmos();
    }
}

