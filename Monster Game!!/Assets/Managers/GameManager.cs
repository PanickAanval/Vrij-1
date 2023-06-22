using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Joeri.Tools.Structure;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private EntityManager m_entities;
    [SerializeField] private ParticleManager m_particles;

    public ParticleManager particles { get => m_particles; }

    private void Awake()
    {
        instance = this;
    }

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
