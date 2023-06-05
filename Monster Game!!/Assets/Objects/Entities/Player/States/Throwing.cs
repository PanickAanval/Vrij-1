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
    public class Throwing : FlexState<Player>
    {
        private Timer m_timer = null;
        private IGrabbable m_grabbingItem = null;

        public Throwing(Player root) : base(root) { }

        public override void OnEnter()
        {
            m_timer = new Timer(m_root.m_grabTime);
            m_root.m_grabbingItem.OnRelease(m_root, m_root.transform.forward * m_root.m_throwStrength);
            m_root.m_grabbingItem = null;

            m_root.m_movement.grip = m_root.m_grabGrip;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
            m_root.m_movement.grip = Mathf.Lerp(m_root.m_grabGrip, m_root.groundGrip, m_timer.percent);

            if (!m_root.m_movement.onGround)
            {
                SwitchToState(typeof(Falling));
                return;
            }
            if (m_timer.HasReached(deltaTime))
            {
                SwitchToState(typeof(Walking));
                return;
            }
        }
    }
}