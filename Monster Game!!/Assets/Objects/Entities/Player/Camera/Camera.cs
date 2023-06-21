using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Utilities;
using Joeri.Tools.Movement;
using Joeri.Tools.Debugging;

public class Camera : MonoBehaviour
{
    [Header("General:")]
    [SerializeField] private float m_followTime = 0.1f;
    [SerializeField] private float m_rotationSpeed = 180f;
    [SerializeField] private float m_rotationGrip = 1f;

    [Header("Direction Adjustment:")]
    [SerializeField] private float m_referenceSpeed = 3f;
    [SerializeField] private float m_adjustmentTime = 3f;

    [Header("Anti-Clipping:")]
    [SerializeField] private LayerMask m_clippingMask;
    [SerializeField] private float m_clippingOffset = 0.1f;

    //  Orbiting data:
    private Orbit m_orbit = new Orbit();
    private Vector3 m_orbitCenter = Vector3.zero;

    //  Tracking the player:
    private Vector3 m_followVelocity = Vector3.zero;
    private Accel.Singular m_yAccel = new Accel.Singular();
    private Accel.Singular m_xAccel = new Accel.Singular();

    //  Adjusting the look direction:
    private Vector2 m_desiredLookDir = Vector2.zero;
    private float m_adjustmentVelocity = 0f;

    //  References:
    private Transform m_target = null;

    //  Events:
    public event CameraOverride onPosOver = null;

    public void Setup(Transform target)
    {
        var offsetFromTarget = transform.position - target.position;

        m_orbitCenter = target.position;
        m_orbit.SetOffset(offsetFromTarget);
        m_target = target;

        SetDesiredDir(Vectors.VectorToFlat(offsetFromTarget));
    }

    public void Tick(Vector2 input, Vector2 playerVel, float deltaTime)
    {
        //  Position:
        transform.position = GetDesiredPosition(input, playerVel, deltaTime);


        //  Rotation:
        transform.rotation = Quaternion.LookRotation(m_target.position - transform.position, Vector3.up);
    }

    /// <returns>The desired position of the camera.</returns>
    private Vector3 GetDesiredPosition(Vector2 input, Vector2 playerVel, float deltaTime)
    {
        m_orbitCenter = Vector3.SmoothDamp(m_orbitCenter, m_target.position, ref m_followVelocity, m_followTime);

        var position = m_orbitCenter + GetDesiredOffset(input, playerVel, deltaTime);

        if (onPosOver != null)
        {
            position = onPosOver.Invoke(position);
        }
        if (Physics.Raycast(m_orbitCenter, position - m_orbitCenter, out RaycastHit hit, m_orbit.distance, m_clippingMask, QueryTriggerInteraction.Ignore))
        {
            position = hit.point + hit.normal * m_clippingOffset;
        }
        return position;
    }

    /// <returns>The desired offset from the player.</returns>
    private Vector3 GetDesiredOffset(Vector2 input, Vector2 playerVel, float deltaTime)
    {
        //  Save the player's last recorded horizontal velocity.
        if (playerVel != Vector2.zero)
        {
            SetDesiredDir(-playerVel);
        }

        var yAngle = GetYAngle(input.x);
        var xAngle = GetXAngle(input.y);

        ///<returns>The desired Y angle from the target to the camera.</returns>
        float GetYAngle(float xInput)
        {
            var angle = m_orbit.yAngle;

            //  Apply the manual rotation offset to the current angle.
            angle -= m_yAccel.CalculateVelocity(xInput, m_rotationSpeed, m_rotationGrip, deltaTime) * deltaTime;
            angle = Mathf.Repeat(angle, 360f);

            //  Calculate adjustment factor.
            var adjustmentFactor = Mathf.Clamp01(Vector2.Dot(Vectors.VectorToFlat(m_orbit.direction), m_desiredLookDir) + 1);
            if (m_referenceSpeed > 0) adjustmentFactor *= Mathf.Clamp01(playerVel.magnitude / m_referenceSpeed);

            //  Apply adjustment factor to the rotation.
            var desiredAngle = Mathf.LerpAngle(angle, Vectors.VectorToAngle(m_desiredLookDir), adjustmentFactor);
            angle = Mathf.SmoothDampAngle(angle, desiredAngle, ref m_adjustmentVelocity, m_adjustmentTime);

            return angle;
        }

        ///<returns>The desired X angle from the target to the camera.</returns>
        float GetXAngle(float yInput)
        {
            var angle = m_orbit.xAngle;

            //  Apply the manual rotation offset to the current angle.
            angle += m_xAccel.CalculateVelocity(yInput, m_rotationSpeed, m_rotationGrip, deltaTime) * deltaTime;
            angle = Mathf.Clamp(angle, -80f, 80f);

            return angle;
        }

        //  Debug.Log($"Current orientation: Horizontal:{yAngle}, Vertical: {xAngle}");

        m_orbit.SetAngles(yAngle, xAngle, m_orbit.distance);
        return m_orbit.offset;
    }

    ///<summary>
    ///Updates the desired direction variable.
    ///</summary>
    void SetDesiredDir(Vector2 vector)
    {
        m_desiredLookDir = vector.normalized;
    }

    public void DrawGizmos()
    {
        if (!Application.isPlaying) return;
        if (m_target == null) return;
        GizmoTools.DrawSphere(m_orbitCenter, m_orbit.distance, Color.white, 0.5f, true, 0.1f);
    }

    public delegate Vector3 CameraOverride(Vector3 position);
}
