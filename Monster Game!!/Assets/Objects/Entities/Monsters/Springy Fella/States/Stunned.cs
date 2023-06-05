using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class SpringyFella
{
    public class Stunned : FlexState<SpringyFella>
    {
        private Timer m_timer = null;

        public Stunned(SpringyFella root) : base(root) { }

        public override void OnEnter()
        {
            m_timer = new Timer(m_root.m_stunnedTime);

            m_root.m_movement.speed = m_root.m_walkSpeed;
            m_root.m_movement.grip = m_root.m_groundGrip;

            m_root.m_movement.gravity = 0f;
            m_root.m_movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            m_root.transform.localEulerAngles += Vector3.up * (360f * deltaTime);

            if (m_timer.HasReached(deltaTime))
            {
                SwitchToState(typeof(Idle));
                return;
            }
            if (!m_root.m_movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
        }

        public override void OnExit()
        {
            m_timer = null;
        }
    }
}
