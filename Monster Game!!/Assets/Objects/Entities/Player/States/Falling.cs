﻿using System;
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
            root.movement.grip = root.m_moveSettings.airGrip;
            root.movement.gravity = root.m_moveSettings.baseGravity;
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyInput(root.m_input, deltaTime);

            if (root.movement.onGround) 
            { 
                OnLand(); 
                return; 
            }
            if (Input.GetKeyDown(KeyCode.Space) && root.m_airJumpAvailable)
            {
                root.m_airJumpAvailable = false;
                SwitchToState(typeof(Jumping));
                return;
            }
        }

        private void OnLand()
        {
            root.m_airJumpAvailable = true;   //  Testing purposes :))))

            //  Return to walking if the ground does not have any special logic.
            //  Ideally we would want to know if any piece of ground inherits from the IStandable interface, but I have yet to figure out how.
            if (!root.movement.groundInfo.Contains(out SpringyFella[] components))
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