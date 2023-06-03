using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Movement;

public abstract partial class Monster : Entity, IGrabbable
{
    [Header("Monster References:")]
    [SerializeField] protected AgentController m_movement = null;

    //  Run-time:
    private FSM<Monster> m_stateMachine = null;

    //  Cache:
    protected Player m_player = null;

    public virtual void Setup(Player player)
    {
        m_player = player;
        m_stateMachine = new FSM<Monster>(this, typeof(Free), new Free(), new PickedUp());
    }

    public void Tick(float deltaTime)
    {
        m_stateMachine.Tick(deltaTime);
    }

    public abstract void TickSubclass(float deltaTime);

    public virtual void OnGrab(Player player)
    {
        m_stateMachine.SwitchToState<PickedUp>().Setup(player.carrySmoothTime, player.grabber);
    }

    public virtual void OnRelease(Player player)
    {
        m_stateMachine.SwitchToState<Free>();
    }

    public override void DrawGizmos()
    {
        m_movement.DrawGizmos();
    }
}
