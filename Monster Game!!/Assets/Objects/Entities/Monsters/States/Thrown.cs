using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

using Joeri.Tools.Structure;

public abstract partial class Monster
{
    public class Thrown : EntityState<Monster>
    {
        public Thrown(Monster root, Settings settings) : base(root, settings) { }

        public void Setup(Vector3 velocity)
        {
            root.movement.velocity = velocity;
        }

        public override void OnEnter()
        {
            root.movement.grip = 0f;
            root.movement.gravity = root.m_moveSettings.baseGravity;

            base.OnEnter();
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