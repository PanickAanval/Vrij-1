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
    public class Idle : State<SpringyFella>
    {
        public override void OnEnter()
        {
            root.m_movement.speed = root.m_walkSpeed;
            root.m_movement.grip = root.m_groundGrip;

            root.m_movement.gravity = 0f;
            root.m_movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            if (!root.m_movement.onGround)
            {
                SwitchToState<Falling>();
                return;
            }
            if (Vector3.Distance(root.transform.position, root.m_player.transform.position) < root.m_detectionRange)
            {
                SwitchToState<Follow>();
                return;
            }

            root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }

        public override void OnDrawGizmos()
        {
            GizmoTools.DrawOutlinedDisc(root.transform.position, root.m_detectionRange, Color.red, Color.white, 0.25f);
        }
    }
}
