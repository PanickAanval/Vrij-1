using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;

partial class Player
{
    public class Grabbing : FlexState<Player>
    {
        private Timer m_timer = null;

        public Grabbing(Player root, float grabTime) : base(root)
        {
            m_timer = new Timer(grabTime);
        }

        public override void OnEnter()
        {
            m_root.m_grabbing.SetState(GrabbyHandler.State.Active);
            m_root.m_movement.grip = m_root.m_grabGrip;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            m_root.m_grabbing.Tick();
            m_root.m_movement.grip = Mathf.Lerp(m_root.m_grabGrip, m_root.m_groundGrip, m_timer.percent);

            if (!m_root.m_movement.onGround) { SwitchToState<Falling>(); return; }
            //if (Input.GetKeyDown(KeyCode.Space)) { SwitchToState<Jumping>(); return; }
            if (m_timer.HasReached(deltaTime)) { SwitchToState<Walking>(); return; }
        }

        public override void OnDrawGizmos()
        {
            m_root.m_grabbing.DrawGizmos();
        }
    }
}