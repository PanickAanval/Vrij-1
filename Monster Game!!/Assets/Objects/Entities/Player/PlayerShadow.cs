using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShadow : MonoBehaviour
{
    [SerializeField] private float m_normalOffset = 0.01f;
    [SerializeField] private LayerMask m_mask;

    private float m_castRadius = 0.5f;

    public void Setup(CharacterController controller)
    {
        m_castRadius = controller.radius / 2;
    }

    public void Tick(Vector3 playerPos, bool onGround)
    {
        transform.position = GetPosition(playerPos, onGround);
    }

    private Vector3 GetPosition(Vector3 playerPos, bool onGround)
    {
        if (onGround)
        {
            gameObject.SetActive(true);
            return playerPos;
        }
        if (Physics.SphereCast(playerPos + Vector3.up * m_castRadius, m_castRadius, Vector3.down, out RaycastHit hit))
        {
            gameObject.SetActive(true);
            return hit.point + (hit.normal * m_normalOffset);
        }

        //  Disable shadow if no ground is under the player at all.
        gameObject.SetActive(false);
        return Vector3.zero;
    }
}
