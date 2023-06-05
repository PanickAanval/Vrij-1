using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;
using Joeri.Tools;

partial class Player
{
    public class Jumping : FlexState<Player>
    {
        public Jumping(Player root) : base(root) { }

        public override void OnEnter()
        {
            m_root.m_movement.grip = m_root.airGrip;
            m_root.m_movement.gravity = m_root.gravity;
            m_root.m_movement.verticalVelocity = m_root.m_jumpForce;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyInput(m_root.m_input, deltaTime);

            if (m_root.m_movement.velocity.y < 0)
            { 
                SwitchToState(typeof(Falling)); 
                return; 
            }
            if (Input.GetKeyDown(KeyCode.Space) && m_root.m_airJumpAvailable)
            {
                m_root.m_airJumpAvailable = false;
                SwitchToState(typeof(Jumping));
                return;
            }
        }
    }
}