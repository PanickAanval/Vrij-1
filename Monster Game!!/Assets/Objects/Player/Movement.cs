using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools;

public partial class Player
{
    [System.Serializable]
    public class Movement
    {
        public CharacterController controller = null;
        public LayerMask movementLayer;

        private AccelerationLogic.Flat m_horizontal = new AccelerationLogic.Flat();
        private UncontrolledAcceleration m_vertical = new UncontrolledAcceleration(0f, 0f, 0f);

        #region Properties

        public UncontrolledAcceleration vertical { get => m_vertical; }

        public Vector3 velocity { get => new Vector3(m_horizontal.velocity.x, m_vertical.velocity, m_horizontal.velocity.y); }
        public Vector2 horizontalVelocity { get => m_horizontal.velocity; }
        public bool onGround { get; private set; }

        private Vector3 groundCheckOrigin { get => controller.transform.position + (Vector3.up * (controller.radius - controller.skinWidth * 2)); }

        #endregion

        public void Tick(Vector2 input, float speed, float grip, float deltaTime)
        {
            var velocity = m_horizontal.CalculateVelocity3D(input, speed, grip, deltaTime);

            controller.transform.rotation = Quaternion.LookRotation(velocity);

            velocity.y = m_vertical.CalculateVelocity(deltaTime);
            controller.Move(velocity * deltaTime);
            onGround = isOnGround();
        }

        public bool isOnGround()
        {
            if (controller == null) return false;
            if (Physics.OverlapSphere(groundCheckOrigin, controller.radius, movementLayer, QueryTriggerInteraction.Ignore).Length > 0)
            {
                return true;
            }
            return false;
        }

        public void DrawGizmos()
        {
            void DrawGroundCheck(bool onGround)
            {
                GizmoTools.DrawSphere(groundCheckOrigin, controller.radius, onGround ? Color.white : Color.black, 0.5f, true, 0.75f);
            }

            if (Application.isPlaying)
            {
                DrawGroundCheck(onGround);
                m_horizontal.Draw(controller.transform.position, Color.blue, Color.black, 0.75f);
                m_vertical.Draw(controller.transform.position, Vector3.up, Color.green, 0.5f);
            }
            else
            {
                if (controller == null) return;
                DrawGroundCheck(isOnGround());
            }

        }
    }
}
