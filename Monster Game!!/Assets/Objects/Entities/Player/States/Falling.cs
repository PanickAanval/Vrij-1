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
    public class Falling : FlexState<Player>
    {
        public Falling(Player root) : base(root) { }

        public override void OnEnter()
        {
            m_root.m_movement.grip = m_root.m_airGrip;
            m_root.m_movement.gravity = m_root.m_gravity;
        }

        public override void OnTick(float deltaTime)
        {
            m_root.m_movement.ApplyInput(m_root.m_input, deltaTime);

            if (m_root.m_movement.onGround) { OnLand(); return; }
        }

        private void OnLand()
        {
            //  Return to walking if the ground does not have any special logic.
            //  Ideally we would want to know if any piece of ground inherits from the IStandable interface, but I have yet to figure out how.
            if (!m_root.m_movement.groundInfo.Contains(out SpringyFella[] components))
            {
                SwitchToState<Walking>();
                return;
            }

            //  If it does, execute their logic.
            for (int i = 0; i < components.Length; i++)
            {
                components[i].OnStand(m_root);
                return;
            }
        }
    }
}
