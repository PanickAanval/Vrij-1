using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools;
using Joeri.Tools.Utilities;
using Joeri.Tools.Debugging;

public class Camera : MonoBehaviour
{
    [Header("General:")]
    [SerializeField] private float m_followTime = 0.1f;
    [SerializeField] private float m_rotationSpeed = 180f;

    [Header("Direction Adjustment:")]
    [SerializeField] private float m_referenceSpeed = 3f;
    [SerializeField] private float m_adjustmentTime = 3f;

    private CameraRecords m_records = null;
    private Orbit m_orbit = new Orbit();

    private Vector3 m_followVelocity = Vector3.zero;

    private Vector2 m_desiredLookDir = Vector2.zero;
    private float m_adjustmentVelocity = 0f;

    private Transform m_target = null;

    public void Setup(Transform target)
    {
        m_records = new CameraRecords(transform, target);
        m_orbit.SetOffset(m_records.offset);

        m_target = target;

        SetDesiredDir(Vectors.VectorToFlat(m_records.offset));
    }

    public void Tick(Vector2 input, Vector2 playerVel, float deltaTime)
    {
        transform.position = GetDesiredPosition(input, playerVel, deltaTime);
        transform.rotation = Quaternion.LookRotation(m_target.position - transform.position, Vector3.up);
        m_records.Update(transform, m_target);
    }

    /// <returns>The desired position of the camera.</returns>
    private Vector3 GetDesiredPosition(Vector2 input, Vector2 playerVel, float deltaTime)
    {
        var currentPos = transform.position;
        var desiredPos = m_target.position + GetDesiredOffset(input, playerVel, deltaTime);

        currentPos = Vector3.SmoothDamp(currentPos, desiredPos, ref m_followVelocity, m_followTime);
        return desiredPos;
    }

    /// <returns>The desired offset from the player.</returns>
    private Vector3 GetDesiredOffset(Vector2 input, Vector2 playerVel, float deltaTime)
    {
        if (playerVel != Vector2.zero)
        {
            SetDesiredDir(-playerVel);                  //  Save the player's last recorded horizontal velocity.
        }

        var yAngle = GetYAngle(input.x);
        var xAngle = GetXAngle(input.y);

        ///<returns>The desired Y angle from the target to the camera.</returns>
        float GetYAngle(float xInput)
        {
            var angle = m_orbit.yAngle;

            //  Apply the manual rotation offset to the current angle.
            angle = Mathf.Repeat(angle - (xInput * (m_rotationSpeed * deltaTime)), 360f);

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
            angle += yInput * (m_rotationSpeed * deltaTime);
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
        GizmoTools.DrawSphere(m_target.position, m_orbit.distance, Color.white, 0.5f, true, 0.1f);
    }

    /// <summary>
    /// Class containing information about the camera the previous frame.
    /// </summary>
    private class CameraRecords
    {
        public Vector3 position;
        public Vector3 offset;
        public Vector3 upDirection;
        public Vector3 rightDirection;
        public Vector3 playerVelocity;

        public CameraRecords(Transform camera, Transform target)
        {
            Update(camera, target);
        }

        public void Update(Transform camera, Transform target)
        {
            position = camera.position;
            offset = camera.position - target.position;
            upDirection = camera.up;
            rightDirection = camera.right;
        }
    }
}
