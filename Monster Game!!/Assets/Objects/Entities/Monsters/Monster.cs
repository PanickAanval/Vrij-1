using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Movement;

public abstract partial class Monster : Entity, IGrabbable
{
    [Header("Monster States:")]
    [SerializeField] protected Falling.Settings m_falling;
    [SerializeField] protected PickedUp.Settings m_pickedUp;
    [SerializeField] protected Thrown.Settings m_thrown;

    //  Cache:
    protected Player m_player = null;

    public AgentController movement { get => GetMovement<AgentController>(); }

    public virtual void Setup(Player player)
    {
        m_player = player;
        m_movement = new AgentController(gameObject, m_moveSettings);
    }

    public virtual void OnGrab(Player player)
    { 
        m_stateMachine.SwitchToState<PickedUp>().Setup(player.carrySmoothTime, player.grabPivot);
    }

    public virtual void OnRelease(Player player, Vector3 releaseVelocity)
    {
        m_stateMachine.SwitchToState<Thrown>().Setup(releaseVelocity);
    }
}
