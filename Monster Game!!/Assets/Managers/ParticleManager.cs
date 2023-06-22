using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [SerializeField] private GameObject m_poomfPrefab;

    public void SpawnPoomf(Vector3 position)
    {
        Instantiate(m_poomfPrefab, position, Quaternion.identity, transform);
    }
}
