using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class SpringyFella
{
    public class Idle : FlexState<SpringyFella>
    {
        public Idle(SpringyFella root) : base(root) { }

        public override void OnEnter()
        {
            m_root.m_movement.speed = m_root.m_walkSpeed;
            m_root.m_movement.grip = m_root.m_groundGrip;

            m_root.m_movement.gravity = 0f;
            m_root.m_movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);

            if (!m_root.m_movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
            if (Vector3.Distance(m_root.transform.position, m_root.m_player.transform.position) < m_root.m_detectionRange)
            {
                SwitchToState(typeof(Follow));
                return;
            }
        }

        public override void OnDrawGizmos()
        {
            GizmoTools.DrawOutlinedDisc(m_root.transform.position, m_root.m_detectionRange, Color.red, Color.white, 0.25f);
        }
    }
}
