using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;

partial class Player
{
    public class Grabbing : EntityState<Player>
    {
        private Timer m_timer = null;

        public Settings settings { get => GetSettings<Settings>(); }

        public Grabbing(Player root, Settings settings) : base(root, settings) { }

        public void Setup(Vector2 dir)
        {
            root.movement.flatVelocity = dir * settings.speed;
        }

        public override void OnEnter()
        {
            m_timer = new Timer(settings.time);
            root.m_grabHandler.SetState(GrabbyHandler.State.Active);

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyDrag(settings.drag, deltaTime);
            root.m_grabHandler.Tick();

            if (root.m_grabHandler.CaughtSomething(out IGrabbable caughtItem))
            {
                caughtItem.OnGrab(root);
                root.m_grabbingItem = caughtItem;
                root.m_grabHandler.SetState(GrabbyHandler.State.Inactive);
            }

            if (!root.movement.onGround) { SwitchToState(typeof(Falling)); return; }
            if (m_timer.HasReached(deltaTime))
            {
                root.SwitchAnimation(settings.exitAnimation, 0.2f);
                SwitchToState(typeof(Walking));
                return;
            }
        }

        public override void OnExit()
        {
            root.m_grabHandler.SetState(GrabbyHandler.State.Inactive);
            m_timer = null;
        }

        public override void OnDrawGizmos()
        {
            root.m_grabHandler.DrawGizmos();
        }

        [System.Serializable]
        public class Settings : EntityState<Player>.Settings
        {
            public AnimationClip exitAnimation;
            public float speed;
            public float drag;
            public float time;
        }
    }
}