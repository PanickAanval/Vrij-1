using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools;

public class Camera : MonoBehaviour
{
    [SerializeField] private Player m_player = null;
    [SerializeField] private Transform m_target = null;
    [SerializeField] private float distanceFromTarget = 0f;
    [Space]
    [SerializeField] private float m_followTime = 0.1f;
    [SerializeField] private float m_rotationSpeed = 180f;
    [SerializeField] private float m_adjustmentTime = 3f;

    private Vector3 m_followVelocity = Vector3.zero;
    private float m_adjustmentVelocity = 0f;

    private CameraRecords m_records = null;

    private void Start()
    {
        m_records = new CameraRecords(transform, m_target);
    }

    private void Update()
    {
        transform.position = GetDesiredPosition();
        transform.rotation = Quaternion.LookRotation(m_target.position - transform.position, Vector3.up);
        m_records = new CameraRecords(transform, m_target);
    }

    /// <returns>The desired position of the camera.</returns>
    private Vector3 GetDesiredPosition()
    {
        var currentPos = transform.position;
        var desiredPos = m_target.position + GetDesiredOffset(Vector2.zero);

        currentPos = Vector3.SmoothDamp(currentPos, desiredPos, ref m_followVelocity, m_followTime);
        return desiredPos;
    }

    /// <returns>The desired offset from the player.</returns>
    private Vector3 GetDesiredOffset(Vector2 input)
    {
        var offset = m_records.offset;
        var direction = Vector3.forward;
        var yAngle = GetYAngle(input.x);
        var xAngle = GetXAngle(input.y);

        ///<returns>The desired Y angle from the target to the camera.</returns>
        float GetYAngle(float input)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) input--;
            if (Input.GetKey(KeyCode.RightArrow)) input++;

            //  Store the current angle in local variable.
            var currentDir = new Vector2(offset.x, offset.z).normalized;
            var currentAngle = Calc.VectorToAngle(currentDir);

            //  Apply the manual rotation offset to the current angle.
            currentAngle = Mathf.Repeat(currentAngle - (input * (m_rotationSpeed * Time.deltaTime)), 360f);

            //  If the player's velocity isn't zero, de angle gradually changes to that negative of the player's velocity.
            if (m_player.horizontalVelocity != Vector2.zero)
            {
                var targetDir = new Vector2(-m_player.velocity.x, -m_player.velocity.z).normalized;
                var adjustmentFactor = Calc.Reverse01(Mathf.Abs(Vector2.Dot(currentDir, targetDir)));
                var desiredAngle = Calc.VectorToAngle(targetDir);

                desiredAngle = Mathf.LerpAngle(currentAngle, desiredAngle, adjustmentFactor);
                currentAngle = Mathf.SmoothDampAngle(currentAngle, desiredAngle, ref m_adjustmentVelocity, m_adjustmentTime);
            }
            return currentAngle;
        }

        ///<returns>The desired X angle from the target to the camera.</returns>
        float GetXAngle(float input)
        {
            if (Input.GetKey(KeyCode.UpArrow)) input++;
            if (Input.GetKey(KeyCode.DownArrow)) input--;

            //  Calculate both the offset with zero height difference.
            var flatOffset = new Vector3(offset.x, 0f, offset.z);

            //  Store the current angle in local variable.
            var currentAngle = Vector3.SignedAngle(flatOffset, offset, m_records.rightDirection);

            //  Apply the manual rotation offset to the current angle.
            currentAngle = currentAngle + (input * (m_rotationSpeed * Time.deltaTime));
            currentAngle = Mathf.Clamp(currentAngle, -80f, 80f);
            return currentAngle;
        }

        Debug.Log($"Current orientation: Horizontal:{yAngle}, Vertical: {xAngle}");

        ///  We apply the Y and X angles to the direction seperately.
        ///  First, the Y rotation needs to be applied, before we know what the X angle to rotate on will be.
        direction = Quaternion.AngleAxis(yAngle, Vector3.up) * direction;                           //  Rotation Horizontally.
        direction = Quaternion.AngleAxis(xAngle, Vector3.Cross(direction, Vector3.up)) * direction; //  Rotation Vertically.

        return direction * distanceFromTarget;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_target == null) return;
        GizmoTools.DrawSphere(m_target.position, distanceFromTarget, Color.white, 0.5f, true, 0.1f);
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

        public CameraRecords(Transform camera, Transform target)
        {
            position        = camera.position;
            offset          = camera.position - target.position;
            upDirection     = camera.up;
            rightDirection  = camera.right;
        }
    }
}
