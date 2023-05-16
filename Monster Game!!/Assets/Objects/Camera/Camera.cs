using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    [SerializeField] private Transform m_tracking = null;
    [SerializeField] private float distanceFromTarget = 0f;
    [Space]
    [SerializeField] private float m_followTime = 0.1f;

    private Vector3 m_followVelocity = Vector3.zero;

    private void Update()
    {
        transform.position = GetDesiredPosition();
        transform.LookAt(m_tracking);
    }

    private Vector3 GetDesiredPosition()
    {
        /// Let's imagine a sphere around the target we're following, with a radius of the desired distance.
        /// Ideally, we would want the camera no further, or closer than the surface of this sphere.
        /// 
        var sphereSurfacePos = m_tracking.position + ((transform.position - m_tracking.position).normalized * distanceFromTarget);

        sphereSurfacePos = Vector3.SmoothDamp(transform.position, sphereSurfacePos, ref m_followVelocity, m_followTime);
        return sphereSurfacePos;
    }

    private float GetHeightFactor()
    {
        return (transform.position.y - (m_tracking.position.y - distanceFromTarget)) / (distanceFromTarget * 2);
    }

    private void OnDrawGizmosSelected()
    {
        if (m_tracking == null) return;
        Joeri.Tools.GizmoTools.DrawSphere(m_tracking.position, distanceFromTarget, Color.white, 0.5f, true, 0.1f);
        Joeri.Tools.GizmoTools.DrawCircle(
            new Vector3(m_tracking.position.x, transform.position.y, m_tracking.position.z),
            distanceFromTarget * Mathf.Sin(GetHeightFactor() * Mathf.PI),
            Color.white);
    }
}
