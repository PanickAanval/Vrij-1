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
    public class Falling : EntityState<Player>
    {
        public Settings settings { get => GetSettings<Settings>(); }

        public Falling(Player root, Settings settings) : base(root, settings) { }

        public override void OnEnter()
        {
            root.movement.grip = root.m_moveSettings.airGrip;
            root.movement.gravity = root.m_moveSettings.baseGravity * root.m_moveSettings.fallMult;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyInput(root.m_leftInputDir, deltaTime);

            if (root.m_input.dashButtonPressed && root.m_airDashAvailable)
            {
                root.m_airDashAvailable = false;
                SwitchToState<Dashing>().Setup(root.m_leftInputDir);
                return;
            }
            if (root.movement.onGround) { OnLand(); return; }
        }

        private void OnLand()
        {
            root.movement.speed = root.m_moveSettings.baseSpeed;
            root.movement.grip = root.m_moveSettings.baseGrip;
            root.movement.gravity = 0f;
            root.movement.verticalVelocity = 0f;

            root.m_airDashAvailable = true;

            root.SwitchAnimation(settings.landAnimation);
            SwitchToState(typeof(Idle));
        }

        [System.Serializable]
        public class Settings : EntityState<Player>.Settings
        {
            public AnimationClip landAnimation;
        }
    }
}
