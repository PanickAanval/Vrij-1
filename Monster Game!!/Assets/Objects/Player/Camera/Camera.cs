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
    private Vector3 m_dirFromTarget = Vector3.zero;

    private Vector3 offsetFromTarget { get => transform.position - m_target.position; }
    private float m_adjustmentVelocity = 0f;

    private void Start()
    {
        m_dirFromTarget = offsetFromTarget.normalized;
    }

    private void Update()
    {
        transform.position = GetDesiredPosition();
        transform.LookAt(m_target);
    }

    //  Arbitrary method.. :(
    /*
    private Vector3 GetDesiredPosition()
    {
        /// Let's imagine a sphere around the target we're following, with a radius of the desired distance.
        /// Ideally, we would want the camera no further, or closer than the surface of this sphere.
        /// On top of that, the camera should stay on the same level during this translation.

        var currentPos = transform.position;

        //  First, we should have the camera follow the player with a maximum speed.
        currentPos += Vector3.ClampMagnitude((m_target.position + m_previousOffset) - currentPos, m_maxFollowSpeed * Time.deltaTime);

        //  Then, we will calculate where the camera would fall on the sphere. It is important we do this after having the camera follow the player a bit.
        var sphereSurfacePos = m_target.position + ((currentPos - new Vector3(m_target.position.x, currentPos.y, m_target.position.z)).normalized * distanceFromTarget);

        //  After that, we 'pull' the camera position toward this sphere.
        currentPos = Vector3.SmoothDamp(currentPos, sphereSurfacePos, ref m_followVelocity, m_followTime);
        return currentPos;
    }
    */

    #region Converters



    #endregion

    private Vector3 GetDesiredPosition()
    {
        m_dirFromTarget = GetDesiredDirection(Vector2.zero);

        var currentPos = transform.position;
        var desiredPos = m_target.position + (m_dirFromTarget * distanceFromTarget);

        //currentPos = Vector3.SmoothDamp(currentPos, desiredPos, ref m_followVelocity, m_followTime);
        return desiredPos;
    }

    private float GetHeightFactor()
    {
        return (transform.position.y - (m_target.position.y - distanceFromTarget)) / (distanceFromTarget * 2);
    }

    private Vector3 GetDesiredDirection(Vector2 input)
    {
        ///<returns>The desired Y angle from the target to the camera.</returns>
        float GetYAngle(float input)
        {
            if (Input.GetKey(KeyCode.LeftArrow)) input--;
            if (Input.GetKey(KeyCode.RightArrow)) input++;

            //  Store the current angle in local variable.
            var currentDir = new Vector2(m_dirFromTarget.x, m_dirFromTarget.z).normalized;
            var currentAngle = Calc.VectorToAngle(currentDir);

            //  Apply the manual rotation offset to the current angle.
            currentAngle = Calc.Scroll(currentAngle, -input * (m_rotationSpeed * Time.deltaTime), 360f);

            //  If the player's velocity isn't zero, de angle gradually changes to that negative of the player's velocity.
            if (m_player.velocity != Vector3.zero)
            {
                var targetDir = new Vector2(-m_player.velocity.x, -m_player.velocity.z).normalized;
                var adjustmentFactor = Calc.Reverse01(Mathf.Abs(Vector2.Dot(currentDir, targetDir)));
                var desiredAngle = Calc.VectorToAngle(targetDir);

                desiredAngle = Mathf.LerpAngle(currentAngle, desiredAngle, adjustmentFactor);
                currentAngle = Mathf.SmoothDampAngle(currentAngle, desiredAngle, ref m_adjustmentVelocity, m_adjustmentTime);
            }
            return currentAngle;
        }

        ///<returns>The desired Z angle from the target to the camera.</returns>
        float GetXAngle(float input)
        {
            if (Input.GetKey(KeyCode.UpArrow)) input++;
            if (Input.GetKey(KeyCode.DownArrow)) input--;

            //  Calculate both the offset from the target, and the offset with zero height difference.
            var offset = transform.position - m_target.position;
            var flatOffset = new Vector3(transform.position.x, m_target.position.y, transform.position.z) - m_target.position;

            //  Store the current angle in local variable.
            var currentAngle = Vector3.SignedAngle(flatOffset, offset, Vector3.Cross(flatOffset, Vector3.up));

            //  Apply the manual rotation offset to the current angle.
            currentAngle = Calc.Scroll(currentAngle, input * (m_rotationSpeed * Time.deltaTime), 360f);
            return currentAngle;
        }

        var direction = Vector3.forward;
        var yAngle = GetYAngle(input.x);
        var xAngle = GetXAngle(input.y);

        Debug.Log($"Current orientation: Horizontal:{yAngle}, Vertical: {xAngle}");

        direction = Quaternion.AngleAxis(yAngle, Vector3.up) * direction;                           //  Rotation Horizontally.
        direction = Quaternion.AngleAxis(xAngle, Vector3.Cross(direction, Vector3.up)) * direction; //  Rotation Vertically.
        return direction;
    }

    private void OnDrawGizmosSelected()
    {
        if (m_target == null) return;
        GizmoTools.DrawSphere(m_target.position, distanceFromTarget, Color.white, 0.5f, true, 0.1f);
        GizmoTools.DrawCircle(
            new Vector3(m_target.position.x, transform.position.y, m_target.position.z),
            distanceFromTarget * Mathf.Sin(GetHeightFactor() * Mathf.PI),
            Color.white);
    }
}
