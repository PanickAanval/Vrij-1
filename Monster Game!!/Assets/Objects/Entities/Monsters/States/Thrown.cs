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
            root.movement.velocity = velocity;
        }

        public override void OnEnter()
        {
            root.movement.grip = 0f;
            root.movement.gravity = root.m_moveSettings.baseGravity;
        }

        public override void OnTick(float deltaTime)
        {
            if (root.movement.velocity.y < 0) 
            { 
                SwitchToState(typeof(Falling));
                return; 
            }

            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);
        }
    }
}