using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class Monster
{
    public class Falling : FlexState<Monster>
    {
        private System.Type m_returnState = null;

        public Falling(Monster root, System.Type returnState) : base(root)
        {
            m_returnState = returnState;
        }

        public override void OnEnter()
        {
            m_root.m_movement.grip = m_root.airGrip;
            m_root.m_movement.gravity = m_root.gravity;
        }

        public override void OnTick(float deltaTime)
        {
            if (m_root.m_movement.onGround)
            {
                SwitchToState(m_returnState);
                return;
            }

            m_root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }
    }
}

