using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

public partial class Player
{
    public class Launched : FlexState<Player>
    {
        public Launched(Player root) : base(root) { }

        public void Setup(float launcPower)
        {
            m_root.m_movement.verticalVelocity = launcPower;
        }

        public override void OnEnter()
        {
            m_root.m_movement.grip = m_root.m_airGrip;
            m_root.m_movement.gravity = m_root.m_gravity;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyInput(m_root.m_input, deltaTime);

            if (m_root.m_movement.velocity.y < 0)
            {
                SwitchToState<Falling>(); 
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