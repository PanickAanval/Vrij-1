using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Utilities;
using Joeri.Tools.Structure;

partial class Player
{
    public class Dashing : EntityState<Player>
    {
        //  Properties:
        private Vector2 m_direction = Vector2.zero;
        private float m_speed = 0f;
        private bool m_preparingForExit = false;

        //  Components:
        private Timer m_timer = null;

        public Settings settings { get => GetSettings<Settings>(); }

        public Dashing(Player root, Settings settings) : base(root, settings) { }

        public void Setup(Vector2 dir)
        {
            m_direction = dir != Vector2.zero ? dir : Vectors.VectorToFlat(root.transform.forward);
            root.velocity = Vectors.FlatToVector(m_direction * m_speed, 0f);
            root.transform.rotation = Quaternion.LookRotation(root.velocity, Vector3.up);
        }

        public override void OnEnter()
        {
            m_speed = settings.time > 0f ? settings.distance / settings.time : settings.distance;
            m_timer = new Timer(settings.time);

            root.enableDashEffects = true;

            base.OnEnter();
        }

        public override void OnTick(float deltaTime)
        {
            root.controller.Move(root.velocity * deltaTime);

            if (!m_preparingForExit && m_timer.percent > 0.8f)
            {
                root.m_animator.Play(settings.animation.name, -1, 0.8f);
            }

            if (!m_timer.HasReached(deltaTime)) return;
            if (root.movement.onGround) SwitchToState(typeof(Idle));
            else                        SwitchToState(typeof(Falling));
        }

        public override void OnExit()
        {
            m_direction = Vector2.zero;
            m_speed = 0f;

            root.flatVelocity = root.flatVelocity.normalized * settings.exitSpeed;
            root.enableDashEffects = false;

            m_timer = null;
        }

        [System.Serializable]
        public class Settings : EntityState<Player>.Settings
        {
            public float distance = 3f;
            public float time = 0.25f;
            [Space]
            public float exitSpeed = 3f;
        }
    }
}