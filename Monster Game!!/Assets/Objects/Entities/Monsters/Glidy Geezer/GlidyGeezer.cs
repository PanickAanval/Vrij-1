﻿using System.Collections;
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

    //  Run-time:
    //private Swapper<float> m_gripSwapper = null;

    public override void Setup(Player player)
    {
        base.Setup(player);
        //m_gripSwapper = new Swapper<float>(m_moveSettings.baseGravity);
        m_stateMachine = new FSM
            (
                typeof(Rotating),
                new Falling(this, typeof(Rotating)),
                new PickedUp(this),
                new Thrown(this),
                new Rotating(this)
            );
    }

    public override void OnGrab(Player player)
    {
        m_stateMachine.SwitchToState<PickedUp>().Setup(player.carrySmoothTime, player.grabPivot);
        //player.move = m_gripSwapper.Swap(player.gravity);
    }

    public override void OnRelease(Player player, Vector3 releaseVelocity)
    {
        m_stateMachine.SwitchToState<Thrown>().Setup(releaseVelocity);
        //player.gravity = m_gripSwapper.Swap(player.gravity);
    }
}
