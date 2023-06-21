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

    [Header("Springy Fella References:")]
    [SerializeField] private PlayerLauncher m_launcher;

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
}

