using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;
using Joeri.Tools.Movement;

public partial class GlidyGeezer : Monster
{
    [Header("Glidy Geezer Properties:")]
    [SerializeField] private float m_rotationTime = 3f;
    [SerializeField] private float m_rotateAmount = 3f;

    [Header("Glidy Geezer States:")]
    [SerializeField] protected Rotating.Settings m_rotating;
    [SerializeField] protected Jumping.Settings m_jumping;

    //  Run-time:
    private Swapper<float> m_multSwapper = null;

    public override void Setup(Player player)
    {
        base.Setup(player);
        m_multSwapper = new Swapper<float>(m_moveSettings.fallMult);
        m_stateMachine = new FSM
            (
                typeof(Jumping),
                new Jumping(this, m_jumping),
                new Falling(this, m_falling, typeof(Rotating)),
                new PickedUp(this, m_pickedUp),
                new Thrown(this, m_thrown),
                new Rotating(this, m_rotating)
            );
    }

    public override void OnGrab(Player player)
    {
        base.OnGrab(player);
        player.moveSettings.fallMult = m_multSwapper.Swap(player.moveSettings.fallMult);
    }

    public override void OnRelease(Player player, Vector3 releaseVelocity)
    {
        base.OnRelease(player, releaseVelocity);
        player.moveSettings.fallMult = m_multSwapper.Swap(player.moveSettings.fallMult);
    }
}

