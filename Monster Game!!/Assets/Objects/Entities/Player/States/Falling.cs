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
    public class Falling : State<Player>
    {
        public override void OnEnter()
        {
            root.m_movement.grip = root.m_airGrip;
            root.m_movement.gravity = root.m_gravity;
        }

        public override void OnTick(float deltaTime)
        {
            root.m_movement.ApplyInput(root.m_input, deltaTime);

            if (root.m_movement.onGround) { OnLand(); return; }
        }

        private void OnLand()
        {
            //  Return to walking if the ground does not have any special logic.
            //  Ideally we would want to know if any piece of ground inherits from the IStandable interface, but I have yet to figure out how.
            if (!root.m_movement.groundInfo.Contains(out SpringyFella[] components))
            {
                SwitchToState<Walking>();
                return;
            }

            //  If it does, execute their logic.
            for (int i = 0; i < components.Length; i++)
            {
                components[i].OnStand(root);
                return;
            }
        }
    }
}
