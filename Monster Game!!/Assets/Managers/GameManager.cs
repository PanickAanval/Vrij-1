using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private EntityManager m_entities;

    private void Start()
    {
        m_entities.Setup();
    }

    private void Update()
    {
        var deltaTime = Time.deltaTime;

        m_entities.Tick(deltaTime);   
    }

    private void OnDrawGizmos()
    {
        m_entities.DrawGizmos();
    }
}
