using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Movement;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class SpringyFella
{
    public class Follow : FlexState<SpringyFella>
    {
        public Follow(SpringyFella root) : base(root) { }

        public override void OnEnter()
        {
            m_root.m_movement.SetBehaviors(new Pursue(m_root.m_lookAheadTime, m_root.m_player.transform));
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyBehaviorVelocity(deltaTime);

            if (!m_root.m_movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
            if (Vector3.Distance(m_root.transform.position, m_root.m_player.transform.position) > m_root.m_detectionRange)
            {
                SwitchToState(typeof(Idle));
                return;
            }
        }

        public override void OnExit()
        {
            m_root.m_movement.ClearBehaviors();
        }

        public override void OnDrawGizmos()
        {
            GizmoTools.DrawOutlinedDisc(m_root.transform.position, m_root.m_detectionRange, Color.blue, Color.white, 0.1f);
        }
    }
}