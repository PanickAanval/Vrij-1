using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Debugging;

namespace Joeri.Tools.Movement
{
    public abstract class MovementBase
    {
        public float speed = 10f;
        public float grip = 5f;
        [Space]
        [SerializeField] private float m_gravity = -9.81f;
        [Space]
        public CharacterController controller = null;
        public LayerMask movementLayer;

        protected Accel.Flat m_horizontal = new Accel.Flat();
        protected Accel.Uncontrolled m_vertical = new Accel.Uncontrolled(0f, 0f, 0f);

        #region Properties

        public Vector3 velocity { get => new Vector3(m_horizontal.velocity.x, m_vertical.velocity, m_horizontal.velocity.y); }
        public Vector2 horizontalVelocity { get => m_horizontal.velocity; }
        public bool onGround { get; protected set; }

        public Accel.Flat horizontal { get => m_horizontal; }
        public Accel.Uncontrolled vertical { get => m_vertical; }

        protected Vector3 groundCheckOrigin
        {
            get
            {
                var position = controller.transform.position + controller.center;

                position += Vector3.down * controller.height / 2;                           //  Go to the bottom of the controller's collider.
                position += Vector3.up * (controller.radius - controller.skinWidth * 2);    //  Set the center so that the overlap slightly reaches under the collider.
                return position;
            }
        }

        #endregion

        public void ApplyDesiredVelocity(Vector2 desiredVelocity, float deltaTime)
        {
            var newVelocity = Vector3.zero;

            //  Calculating velocity in the horizontal class.
            m_horizontal.CalculateVelocity(desiredVelocity, grip, deltaTime);

            //  Applying calculations.
            newVelocity.x = m_horizontal.velocity.x;
            newVelocity.z = m_horizontal.velocity.y;

            //  Applying rotation, before vertical velocity gets calculated.
            if (newVelocity != Vector3.zero) controller.transform.rotation = Quaternion.LookRotation(newVelocity);

            //  Calculating, and applying vertical velocity.
            newVelocity.y = m_vertical.CalculateVelocity(deltaTime);

            //  Misc.
            controller.Move(newVelocity * deltaTime);
            onGround = isOnGround();
        }

        /// <returns>True if the player is standing on valid ground. Calculated by a Physics.OverlapSphere(...).</returns>
        public bool isOnGround()
        {
            if (controller == null) return false;
            if (Physics.OverlapSphere(groundCheckOrigin, controller.radius, movementLayer, QueryTriggerInteraction.Ignore).Length > 0)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// Draws the functionality of the movement class as shapes in 3D space. Representing their values and states.
        /// </summary>
        public virtual void DrawGizmos()
        {
            GizmoTools.DrawSphere(groundCheckOrigin, controller.radius, onGround ? Color.white : Color.black, 0.5f, true, 0.75f);
            m_horizontal.Draw(controller.transform.position, Color.blue, Color.black, 0.75f);
            m_vertical.Draw(controller.transform.position, Vector3.up, Color.green, 0.5f);
        }

        /// <summary>
        /// Sets the gravity state of the movement class to the desired state.
        /// </summary>
        public void SetGravityState(bool enabled)
        {
            if (enabled) m_vertical.acceleration = m_gravity;
            else m_vertical.acceleration = 0f;
        }
    }
}
