using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Joeri.Tools.Utilities;
using Joeri.Tools.Debugging;

namespace Joeri.Tools.Movement
{
    [System.Serializable]
    public abstract class MovementBase
    {
        [HideInInspector] public bool rotate = true;    //  Temporary?
        [HideInInspector] public float speed = 10f;
        [HideInInspector] public float grip = 5f;

        [SerializeField] private CharacterController m_controller = null;
        [SerializeField] private LayerMask m_movementLayer;

        protected Accel.Flat m_horizontal = new Accel.Flat();
        protected Accel.Uncontrolled m_vertical = new Accel.Uncontrolled(0f, 0f, 0f);

        protected bool m_onGround = false;
        protected GroundInfo m_groundInfo = null;

        #region Properties

        public float gravity
        {
            get => m_vertical.acceleration;
            set => m_vertical.acceleration = value;
        }

        public CharacterController controller { get => m_controller; }

        public Vector3 velocity 
        { 
            get => new Vector3(m_horizontal.velocity.x, m_vertical.velocity, m_horizontal.velocity.y);
            set
            {
                m_horizontal.velocity.x = value.x;
                m_horizontal.velocity.y = value.z;
                m_vertical.velocity = value.y;
            }
        }
        public Vector2 flatVelocity 
        { 
            get => m_horizontal.velocity;
            set => m_horizontal.velocity = value;
        }
        public float verticalVelocity
        {
            get => m_vertical.velocity;
            set => m_vertical.velocity = value;
        }

        public bool onGround { get => m_onGround; }
        public GroundInfo groundInfo { get => m_groundInfo; }


        protected Vector3 groundCheckOrigin
        {
            get
            {
                var position = m_controller.transform.position + m_controller.center;

                position += Vector3.down * m_controller.height / 2;                             //  Go to the bottom of the controller's collider.
                position += Vector3.up * (m_controller.radius - m_controller.skinWidth * 2);    //  Set the center so that the overlap slightly reaches under the collider.
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
            if (newVelocity != Vector3.zero && rotate) m_controller.transform.rotation = Quaternion.LookRotation(newVelocity);

            //  Calculating, and applying vertical velocity.
            newVelocity.y = m_vertical.CalculateVelocity(deltaTime);

            //  Misc.
            m_controller.Move(newVelocity * deltaTime);
            m_onGround = isOnGround(out GroundInfo groundInfo);
            m_groundInfo = groundInfo;
        }

        /// <returns>True if the player is standing on valid ground. Calculated by a Physics.OverlapSphere(...).</returns>
        public bool isOnGround(out GroundInfo info)
        {
            info = null;

            if (m_controller == null) return false;

            var overlappingColliders = Physics.OverlapSphere(groundCheckOrigin, m_controller.radius, m_movementLayer, QueryTriggerInteraction.Ignore);

            if (overlappingColliders.Length > 0)
            {
                info = new GroundInfo(overlappingColliders);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Draws the functionality of the movement class as shapes in 3D space. Representing their values and states.
        /// </summary>
        public virtual void DrawGizmos()
        {
            GizmoTools.DrawSphere(groundCheckOrigin, m_controller.radius, onGround ? Color.white : Color.black, 0.5f, true, 0.75f);
            m_horizontal.Draw(m_controller.transform.position, Color.blue, Color.black, 0.75f);
            m_vertical.Draw(m_controller.transform.position, Vector3.up, Color.green, 0.5f);
        }

        //  Adds the given velocity to the movement.
        public void AddVelocity(Vector3 velocity)
        {
            m_horizontal.velocity.x += velocity.x;
            m_horizontal.velocity.y += velocity.z;
            m_vertical.velocity += velocity.y;
        }

        /// <summary>
        /// Class holding info of the ground you're currently stnading on.
        /// </summary>
        public class GroundInfo
        {
            public readonly Collider[] colliders = null;

            public GroundInfo(Collider[] interactingGroundColliders)
            {
                colliders = interactingGroundColliders;
            }

            public bool Contains<T>(out T[] containingComponents)
            {
                return Util.Contains(out containingComponents, colliders);
            }
        }
    }
}
