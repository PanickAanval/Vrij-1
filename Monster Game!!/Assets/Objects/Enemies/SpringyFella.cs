using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Movement;

public partial class SpringyFella : MonoBehaviour
{
    [SerializeField] private AgentController m_movement;

    [Header("States:")]
    [SerializeField] private Idle.Settings m_idle;
    [SerializeField] private Follow.Settings m_follow;
    [SerializeField] private Falling.Settings m_falling;

    private FSM<SpringyFella> m_stateMachine;
    private Player m_player = null;

    public void Setup(Player player)
    {
        m_stateMachine = new FSM<SpringyFella>(this, typeof(Idle), new Idle(m_idle), new Follow(m_follow), new Falling(m_falling));
        m_player = player;
    }

    public void Tick(float deltaTime)
    {
        m_stateMachine.Tick(deltaTime);
    }

    public void DrawGizmos()
    {
        m_stateMachine.DrawGizmos(transform.position);
        m_movement.DrawGizmos();
    }
}
