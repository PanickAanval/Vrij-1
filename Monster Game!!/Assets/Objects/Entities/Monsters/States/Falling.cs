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
    public class Falling : EntityState<Monster>
    {
        private System.Type m_returnState = null;

        public Falling(Monster root, Settings settings, System.Type returnState) : base(root, settings)
        {
            m_returnState = returnState;
        }

        public override void OnEnter()
        {
            root.movement.grip      = root.m_moveSettings.airGrip;
            root.movement.gravity   = root.m_moveSettings.baseGravity * root.m_moveSettings.fallMult;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            if (root.movement.onGround)
            {
                GameManager.instance.particles.SpawnPoomf(root.transform.position);

                SwitchToState(m_returnState); 
                return; 
            }
            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }
    }
}

