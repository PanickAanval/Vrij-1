using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class GlidyGeezer
{
    public class Rotating : FlexState<GlidyGeezer>
    {
        private Timer m_timer = null;

        public Rotating(GlidyGeezer root) : base(root) { }

        public override void OnEnter()
        {
            m_timer = new Timer(m_root.m_rotationTime);

            m_root.m_movement.rotate = false;
            m_root.m_movement.speed = m_root.walkSpeed;
            m_root.m_movement.grip = m_root.groundGrip;

            m_root.m_movement.gravity = 0f;
            m_root.m_movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            m_root.transform.localEulerAngles += Vector3.up * ((m_root.m_rotateAmount / m_root.m_rotationTime) * deltaTime);

            if (!m_root.m_movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
            if (m_timer.HasReached(deltaTime))
            {
                m_root.m_movement.velocity = (m_root.transform.forward + Vector3.up) * m_root.m_jumpForce;
                return;
            }
        }

        public override void OnExit()
        {
            m_root.m_movement.rotate = true;
            m_timer = null;
        }
    }
}
