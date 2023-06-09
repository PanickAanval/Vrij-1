﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Structure;

partial class Player
{
    public class Throwing : EntityState<Player>
    {
        private Timer m_timer = null;
        private IGrabbable m_grabbingItem = null;

        public Settings settings { get => GetSettings<Settings>(); }

        public Throwing(Player root, Settings settings) : base(root, settings) { }

        public override void OnEnter()
        {
            m_timer = new Timer(settings.time);
            root.m_grabbingItem.OnRelease(root, (root.transform.forward * settings.strength) + root.velocity);
            root.m_grabbingItem = null;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            root.movement.ApplyDrag(settings.drag, deltaTime);

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
            m_timer = null;
        }

        [System.Serializable]
        public class Settings : EntityState<Player>.Settings
        {
            public AnimationClip exitAnimation;
            public float strength;
            public float drag;
            public float time;
        }
    }
}