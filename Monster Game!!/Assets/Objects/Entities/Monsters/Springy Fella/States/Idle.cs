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
    public class Idle : EntityState<SpringyFella>
    {
        public Idle(SpringyFella root,Settings settings) : base(root, settings) { }

        public override void OnEnter()
        {
            root.movement.speed = root.m_moveSettings.baseSpeed;
            root.movement.grip = root.m_moveSettings.baseGrip;
            root.movement.gravity = 0f;
            root.movement.verticalVelocity = 0f;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);

            if (!root.movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
            if (Vector3.Distance(root.transform.position, root.m_player.transform.position) < root.m_detectionRange)
            {
                SwitchToState(typeof(Follow));
                return;
            }
        }

        public override void OnDrawGizmos()
        {
            GizmoTools.DrawOutlinedDisc(root.transform.position, root.m_detectionRange, Color.red, Color.white, 0.25f);
        }
    }
}
