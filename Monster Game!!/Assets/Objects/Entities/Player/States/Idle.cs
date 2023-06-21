using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Structure;

partial class Player
{
    public class Idle : EntityState<Player>
    {
        public Idle(Player root, Settings settings) : base(root, settings) { }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyInput(root.m_leftInputDir, deltaTime);

            if (root.m_input.leftInput != Vector2.zero) { SwitchToState(typeof(Walking));                       return; }
            if (root.m_input.jumpButtonPressed)         { SwitchToState(typeof(Jumping));                       return; }
            if (root.m_input.dashButtonPressed)         { SwitchToState<Dashing>().Setup(root.m_leftInputDir);  return; }
            if (!root.movement.onGround)                { SwitchToState(typeof(Falling));                       return; }
            if (root.m_input.grabButtonPressed)
            {
                if (root.m_grabbingItem == null)
                {
                    SwitchToState(typeof(Grabbing));
                    return;
                }
                SwitchToState(typeof(Throwing));
                return;
            }
        }
    }
}
