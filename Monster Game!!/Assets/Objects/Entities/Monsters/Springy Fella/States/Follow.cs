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
            root.movement.SetBehaviors(new Pursue(root.m_lookAheadTime, root.m_player.transform));
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyBehaviorVelocity(deltaTime);

            if (!root.movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
            if (Vector3.Distance(root.transform.position, root.m_player.transform.position) > root.m_detectionRange)
            {
                SwitchToState(typeof(Idle));
                return;
            }
        }

        public override void OnExit()
        {
            root.movement.ClearBehaviors();
        }

        public override void OnDrawGizmos()
        {
            GizmoTools.DrawOutlinedDisc(root.transform.position, root.m_detectionRange, Color.blue, Color.white, 0.1f);
        }
    }
}