using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Joeri.Tools.Structure;

public abstract partial class Monster
{
    public class Thrown : FlexState<Monster>
    {
        public Thrown(Monster root) : base(root) { }

        public void Setup(Vector3 velocity)
        {
            m_root.m_movement.velocity = velocity;
        }

        public override void OnEnter()
        {
            m_root.m_movement.grip = 0f;
            m_root.m_movement.gravity = m_root.m_gravity;
        }

        public override void OnTick(float deltaTime)
        {
            if (m_root.m_movement.velocity.y < 0) 
            { 
                SwitchToState(typeof(Falling));
                return; 
            }

            m_root.m_movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }
    }
}