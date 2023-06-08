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
            m_timer = new Timer(root.m_stunnedTime);

            root.movement.canRotate = false;
            root.movement.speed = root.m_moveSettings.baseSpeed;
            root.movement.grip = root.m_moveSettings.baseGrip;
            root.movement.gravity = 0f;
            root.movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            root.transform.localEulerAngles += Vector3.up * (360f * deltaTime);

            if (m_timer.HasReached(deltaTime))
            {
                SwitchToState(typeof(Idle));
                return;
            }
            if (!root.movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
        }

        public override void OnExit()
        {
            root.movement.canRotate = true;
            m_timer = null;
        }
    }
}
