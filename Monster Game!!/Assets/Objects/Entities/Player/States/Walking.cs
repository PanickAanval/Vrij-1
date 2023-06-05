using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

partial class Player
{
    public class Walking : FlexState<Player>
    {
        public Walking(Player root) : base(root) { }

        public override void OnEnter()
        {
            m_root.m_movement.speed = m_root.m_walkSpeed;
            m_root.m_movement.grip = m_root.m_groundGrip;

            m_root.m_movement.gravity = 0f;
            m_root.m_movement.verticalVelocity = 0f;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyInput(m_root.m_input, deltaTime);

            if (!m_root.m_movement.onGround) { SwitchToState<Falling>(); return; }
            if (Input.GetKeyDown(KeyCode.Space)) { SwitchToState<Jumping>(); return; }
            if (Input.GetKeyDown(KeyCode.F)) { SwitchToState<Grabbing>(); return; }
        }
    }
}
