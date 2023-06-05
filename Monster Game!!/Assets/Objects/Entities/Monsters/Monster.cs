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

    //  Cache:
    protected Player m_player = null;

    public virtual void Setup(Player player)
    {
        m_player = player;
    }

    public abstract void Tick(float deltaTime);

    public virtual void OnGrab(Player player) { }

    public virtual void OnRelease(Player player, Vector3 releaseVelocity) { }

    public override void DrawGizmos()
    {
        m_movement.DrawGizmos();
    }
}
