using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;
using Joeri.Tools.Debugging;

public partial class SpringyFella
{
    public class Launching : EntityState<SpringyFella>
    {
        private Timer m_timer = null;

        public Settings settings { get => GetSettings<Settings>(); }

        public Launching(SpringyFella root, Settings settings) : base(root, settings) { }

        public override void OnEnter()
        {
            m_timer = new Timer(settings.duration);

            root.m_animator.Play(settings.animation.name, -1, 0.5f);
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyDesiredVelocity(Vector2.zero, deltaTime);

            if (m_timer.HasReached(deltaTime))  { SwitchToState(typeof(Idle));      return; }
            if (!root.movement.onGround)        { SwitchToState(typeof(Falling));   return; }
        }

        public override void OnExit()
        {
            m_timer = null;
        }

        [System.Serializable]
        public class Settings : EntityState<SpringyFella>.Settings
        {
            public float duration;
        }
    }
}
